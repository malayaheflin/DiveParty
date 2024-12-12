using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OctopusArm : MonoBehaviour
{
    [Header("Metal")]
    private Animator anim;
    public RuntimeAnimatorController anim_NoMetal;
    public Collider2D colliderAtk;
    public int health = 3;
    public bool isMetalRemoved;

    // Attack
    private bool isCheckHit;
    private bool hasHit;

    [Header("Appear & Dead")]
    public Transform hidePos;
    public Vector3 showPos;
    public float moveTime = 2f;
    public float attackBreak = 3f;
    private bool isDead;
    private bool isAppearing;

    private void Start()
    {
        anim = GetComponent<Animator>();
        showPos = this.transform.position;
        this.transform.position = hidePos.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowUp();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !isAppearing)
        {
            GetHurt();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Submarine") && !hasHit)
        {
            hasHit = true;
            ShipMovement.Instance.GetAttacked();
        }
        else if (collision.CompareTag("Beam"))
        {
            GetRidOfMetal();
        }
        else if (collision.CompareTag("Bullet"))
        {
            GetHurt();
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Submarine") && !hasHit)
        {
            hasHit = true;
            ShipMovement.Instance.GetAttacked();
        }
        else if (collision.CompareTag("Beam"))
        {
            GetRidOfMetal();
        }
    }

    private void AttackPlayer()
    {
        // AttackSound?

        anim.SetTrigger("Attack");
        SoundMgr.Instance.PlaySound("sfx_octopus_attack");
    }

    private void GetHurt()
    {
        if (isDead && isAppearing)
            return;

        StopAllCoroutines();

        // Sound Effect

        anim.SetTrigger("Hurt");
        health --;
        if(health < 0)
        {
            isDead = true;
            OctopusBoss.Instance.GetHurt(1);
            StartCoroutine(MoveTowardsPostion(false));

            OctopusBoss.Instance.isArmAttacking = false;
        }
    }

    public void ShowUp()
    {
        health = 3;
        isDead = false;
        StartCoroutine(MoveTowardsPostion(true));

        StartCoroutine(StartAttack());
    }

    public void CheckHitSubmarine()
    {
        StartCoroutine(HitSubmarine());
    }

    private IEnumerator HitSubmarine()
    {
        if (!isCheckHit)
        {
            isCheckHit = true;
            colliderAtk.enabled = true;
            yield return new WaitForSeconds(0.25f);
            colliderAtk.enabled = false;
            isCheckHit = true;
            hasHit = false;
        }
    }

    private IEnumerator MoveTowardsPostion(bool isShow)
    {
        isAppearing = true;
        if (isShow)
        {
            float t = 0;
            while (t <= moveTime)
            {
                t += Time.deltaTime;
                this.transform.position = Vector3.Lerp(hidePos.position, showPos, t / moveTime);
                yield return null;
            }
        }
        else
        {
            float t = 0;
            while (t <= moveTime)
            {
                t += Time.deltaTime;
                this.transform.position = Vector3.Lerp(showPos, hidePos.position, t / moveTime);
                yield return null;
            }
        }
        isAppearing = false;
    }

    private IEnumerator StartAttack()
    {
        // Attack Two times
        yield return new WaitForSeconds(moveTime);

        AttackPlayer();

        yield return new WaitForSeconds(attackBreak);

        AttackPlayer();

        yield return new WaitForSeconds(attackBreak);

        // Then Hide
        yield return MoveTowardsPostion(false);

        OctopusBoss.Instance.isArmAttacking = false;

    }

    private void GetRidOfMetal()
    {
        if (!isMetalRemoved)
        {
            StopAllCoroutines();
            anim.runtimeAnimatorController = anim_NoMetal;
            isMetalRemoved = true;
            OctopusBoss.Instance.isArmAttacking = false;
            OctopusBoss.Instance.CheckAllMetalRemoved();
            // Create Prefabs Of Metal

            StartCoroutine(MoveTowardsPostion(false));
        }

    }


}
