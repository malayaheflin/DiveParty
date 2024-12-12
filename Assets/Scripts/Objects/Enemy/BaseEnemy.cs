using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BaseEnemy : MonoBehaviour
{
    [Header("BaseInfo")]
    public int hp = 1;
    public E_EnemyType type;
    public int money;

    [Header("Behavior")]
    public float movingSpeed = 1;
    public float rotateSpeed = 100;
    public float detectedRange = 3;
    public float attackBreak = 2f;
    public float randomMoveRange = 3f;

    [SerializeField] GameObject treasureDrop;

    // Tags used in decision of behaviour
    protected bool canChase;
    protected bool canAttack;

    // Random Move(Before detect player)
    protected Vector3 randomDirection;
    protected Vector2 originPos;

    // Basic Component
    protected Animator anim;
    protected Collider2D coll;
    protected SpriteRenderer sprite;

    // Tags used in IEnumerator
    protected bool isStun;
    protected bool isMoving;
    protected bool isAttacking;
    protected bool isHurting;
    protected bool isDead;

    //public GameObject TEST;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Submarine"))
        {
            canAttack = true;
            canChase = false;
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Submarine"))
        {
            canAttack = false;
            canChase = true;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            GetHurt();
        }
    }

    /*protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Submarine"))
        {
            canAttack = false;
            canChase = true;
        }
    }*/

    protected virtual void Start()
    {
        coll = this.GetComponent<Collider2D>();
        anim = this.GetComponent<Animator>();
        sprite = this.GetComponent<SpriteRenderer>();
        originPos = ConvertV3ToV2(this.transform.position);
    }

    protected virtual void Update()
    {
        if (isDead || isHurting || isStun)
            return;
        DetectSubmarine();
        if (canChase)
        {
            ChaseSubmarine();
        }
        else
        {
            RandomMove();
        }

        if(canAttack)
            StartCoroutine(Attack());
    }

    protected virtual void RandomMove()
    {
        float rotateAmount = Vector3.Cross(randomDirection, transform.up).z;
        transform.Rotate(0, 0, rotateAmount * rotateSpeed * Time.deltaTime);

        // Move
        transform.Translate(Vector2.up * movingSpeed * Time.deltaTime, Space.Self);
        StartCoroutine(GenerateTargetPos());
    }

    protected virtual IEnumerator GenerateTargetPos()
    {
        if (!isMoving)
        {
            isMoving = true;
            randomDirection = (originPos + Random.insideUnitCircle * randomMoveRange - ConvertV3ToV2(this.transform.position)).normalized;
            yield return new WaitForSeconds(2 * randomMoveRange / movingSpeed);
            isMoving = false;
        }
    }

    protected virtual void ChaseSubmarine()
    {
        Vector3 direction = (ShipMovement.Instance.transform.position - this.transform.position).normalized;
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        transform.Rotate(0, 0, rotateAmount * rotateSpeed * Time.deltaTime);

        // Move
        transform.Translate(Vector2.up * movingSpeed * Time.deltaTime, Space.Self);
    }

    protected virtual void DetectSubmarine()
    {
        if (!canChase && ! canAttack)
        {
            canChase = Vector2.Distance(ConvertV3ToV2(this.transform.position), 
                                        ConvertV3ToV2(ShipMovement.Instance.transform.position)) <= detectedRange;
        }
    }

    protected virtual IEnumerator Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            anim.SetTrigger("Attack");
            // Call Submarine Hurt Function
            ShipMovement.Instance.GetAttacked();

            yield return new WaitForSeconds(attackBreak);
            isAttacking = false;

            print("BaseEnemy_Attack");
        }
    }

    protected virtual void GetHurt()
    {
        hp--;

        if (hp <= 0)
        {
            Dead();
        }
        else
        {
            StartCoroutine(Hurt());
        }
    }

    protected virtual IEnumerator Hurt()
    {
        if (!isHurting)
        {
            isHurting = true;
            anim.SetTrigger("Hurt");
            yield return new WaitForSeconds(1f);
            isHurting = false;
        }
    }

    public void EnemyGetStun()
    {
        if (!isStun)
        {
            StopAllCoroutines();
            StartCoroutine(EnemyStunning());
        }
    }

    protected virtual IEnumerator EnemyStunning()
    {
        
        isStun = true;
        //anim.SetTrigger("Stun");
        Debug.Log("Enemy Get Stun");
        yield return new WaitForSeconds(1f);

        RefreshAllState();
    }

    protected virtual void Dead()
    {
        isDead = true;
        StopAllCoroutines();

        // VFX or Dead Effect
        anim.SetTrigger("Dead");
        coll.enabled = false;

        SoundMgr.Instance.PlaySound("sfx_enemy_dies");

        StartCoroutine(DestroyMyself());
        // PoolMgr.Instance.PushObj(type.ToString(), this.gameObject);
    }

    private IEnumerator DestroyMyself()
    {
        yield return new WaitForSeconds(1f);
        // Create Dead VFX
        Instantiate(treasureDrop, transform.position, Quaternion.identity); // spawn treasure
        Destroy(this.gameObject);
    }

    public void RefreshAllState()
    {
        isAttacking = false;
        isHurting = false ;
        isMoving = false ;
        isStun = false;
    }

    public Vector2 ConvertV3ToV2(Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

}
