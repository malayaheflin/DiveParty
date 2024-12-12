using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class BaseObstacle : MonoBehaviour
{
    public E_ObstacleType obstacleName;
    public int hp = 5;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            SoundMgr.Instance.PlaySound("sfx_explosion_big");
            GetHurt();
        }
    }

    protected virtual void GetHurt()
    {

        hp--;
        if (hp <= 0)
        {
            PoolMgr.Instance.GetObj("explosion07");
            SoundMgr.Instance.PlaySound("sfx_explosion_big");
            Destroy(gameObject);
        }
    }

    public virtual void DestroyMyself()
    {
        // Explosion VFX
        // PoolMgr.Instance.GetObj("", this.transform.position);
        // PoolMgr.Instance.PushObj(obstacleName.ToString(), this.gameObject);
    } 
}
