using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.GetComponent<BaseEnemy>()?.EnemyGetStun();
        Debug.Log("STUN");
    }

    // void OnTriggerStay2D(Collider2D other)
    // {
    //     other.gameObject.GetComponent<BaseEnemy>()?.EnemyGetStun();
    //     Debug.Log("STUN");
    // }
}
