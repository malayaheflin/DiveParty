using System.Collections;
using UnityEngine;

public class SwordFish : BaseEnemy
{
    public float attackTime = 2f;
    public float accelerationSpeed = 5;
    public float maxSpeed = 15f;
    private float realSpeed;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        canAttack = false;
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        canAttack = true;
    }

    protected override void Start()
    {
        base.Start();
        realSpeed = movingSpeed;
    }

    protected override void Update()
    {
        if (isDead || isHurting || isStun)
            return;
        DetectSubmarine();
        // Only in detect range can attack
        if (canChase)
        {
            StartCoroutine(Attack());
        }
        else
        {
            RandomMove();
        }
    }

    protected override void RandomMove()
    {
        float rotateAmount = Vector3.Cross(randomDirection, -transform.up).z;
        transform.Rotate(0, 0, -rotateAmount * rotateSpeed * Time.deltaTime);

        // Move
        transform.Translate(Vector2.down * realSpeed * Time.deltaTime / 5, Space.Self);
        StartCoroutine(GenerateTargetPos());
    }

    protected override IEnumerator Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            anim.SetTrigger("Attack");
            ShipMovement.Instance.GetAttacked();
            yield return new WaitForSeconds(0.5f);

            canAttack = true;

            while (canAttack)
            {
                realSpeed += Time.deltaTime * accelerationSpeed;
                realSpeed = realSpeed > maxSpeed ? maxSpeed : realSpeed;

                // Rotate
                Vector3 direction = (ShipMovement.Instance.transform.position - this.transform.position).normalized;
                float rotateAmount = Vector3.Cross(direction, -transform.up).z;
                transform.Rotate(0, 0, -rotateAmount * rotateSpeed * Time.deltaTime);

                // Move
                transform.Translate(-Vector2.up * realSpeed * Time.deltaTime, Space.Self);
                yield return null;
            }

            realSpeed = movingSpeed;

            // Go far away
            float temp = 0;
            while (temp <= 2f)
            {
                temp += Time.deltaTime;

                Vector3 dir = this.transform.position - ShipMovement.Instance.transform.position;
                float rotate = Vector3.Cross(dir, -transform.up).z;
                transform.Rotate(0, 0, -rotate * rotateSpeed * Time.deltaTime);
                // Move
                transform.Translate(- Vector2.up * realSpeed * Time.deltaTime / 2, Space.Self);
                yield return null;
            }

            isAttacking = false;
        }
    }
}
