using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishInGroup : MonoBehaviour
{
    public int money;
    // In case two bullet enter this at the same time
    private GameObject temp = null;
    [SerializeField] GameObject treasureDrop;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (temp == null)
            {
                temp = gameObject;
                DestroyMyself();
            }
        }
    }

    private void DestroyMyself()
    {
        // GameMgr.Instance.AddMoney(money);
        Instantiate(treasureDrop, transform.position, Quaternion.identity); // spawn treasure
        Destroy(this.gameObject);
    }



}
