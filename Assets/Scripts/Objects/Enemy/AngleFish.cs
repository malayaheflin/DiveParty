using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleFish : BaseEnemy
{
    protected override void RandomMove()
    {
        float rotateAmount = Vector3.Cross(randomDirection, -transform.up).z;
        transform.Rotate(0, 0, -rotateAmount * rotateSpeed * Time.deltaTime);

        // Move
        transform.Translate(Vector2.down * movingSpeed * Time.deltaTime / 2, Space.Self);
        StartCoroutine(GenerateTargetPos());
    }

    protected override void ChaseSubmarine()
    {
        Vector3 direction = (ShipMovement.Instance.transform.position - this.transform.position).normalized;
        float rotateAmount = Vector3.Cross(direction, - transform.up).z;
        transform.Rotate(0, 0, -rotateAmount * rotateSpeed * Time.deltaTime);

        // Move
        transform.Translate(Vector2.down * movingSpeed * Time.deltaTime, Space.Self);
    }

    protected override IEnumerator Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            anim.SetTrigger("Attack");
            // Call Submarine Hurt Function
            ShipMovement.Instance.GetAttacked();

            yield return new WaitForSeconds(attackBreak);

            //SoundMgr.Instance.PlaySound("sfx_anglerfish_attack");
            isAttacking = false;
        }
    }
}
