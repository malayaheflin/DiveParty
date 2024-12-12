using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Crab : BaseEnemy
{
    [SerializeField]
    private Transform leftPos;
    [SerializeField]
    private Transform rightPos;

    private bool isMovingLeft;

    protected override void Start()
    {
        base.Start();
        this.transform.DetachChildren();
    }

    protected override void Update()
    {
        if (canAttack)
        {
            StartCoroutine(Attack());
        }
        else
        {
            RandomMove();
        }

            
    }

    //protected override void ChaseSubmarine()
    //{
    //    Vector3 direction = (ShipMovement.Instance.transform.position - this.transform.position).normalized;

    //    this.transform.Translate(direction * movingSpeed * Time.deltaTime);

    //    Debug.Log("Crab Chasing");
    //}

    protected override void RandomMove() //Fix movement between two positions
    {
        if (isMovingLeft)
        {
            this.transform.Translate(Vector3.left * movingSpeed * Time.deltaTime);
            if(this.transform.position.x <= leftPos.position.x)
                isMovingLeft = false;
        }
        else
        {
            this.transform.Translate(Vector3.right * movingSpeed * Time.deltaTime);
            if (this.transform.position.x > rightPos.position.x)
                isMovingLeft = true;
        }
    }

    protected override IEnumerator Attack()
    {
        if(this.transform.position.x > ShipMovement.Instance.transform.position.x)
            sprite.flipX = true;
        else
            sprite.flipX = false;
        if (!isAttacking)
        {
            isAttacking = true;
            anim.SetTrigger("Attack");
            // Call Submarine Hurt Function
            ShipMovement.Instance.GetAttacked();

            yield return new WaitForSeconds(attackBreak);

            SoundMgr.Instance.PlaySound("sfx_crab_snapping");
            isAttacking = false;
            print("Crab_Attack");
        }
    }
}
