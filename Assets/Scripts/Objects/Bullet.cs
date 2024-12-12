using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // VFX SFX Created here 
        if (!collision.CompareTag("Beam") && !collision.CompareTag("BulletDontCollide"))
            DestroyMyself();
    }

    private void DestroyMyself()
    {
        SoundMgr.Instance.PlaySound("sfx_explosion_small");
        PoolMgr.Instance.GetObj("explosion01", this.transform.position);
        Destroy(this.gameObject);
    }
}
