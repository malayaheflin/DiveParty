using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseMagnetic : MonoBehaviour
{
    private float speed = 2f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Beam"))
        {
            Vector3 dir = (ShipMovement.Instance.transform.position - transform.position).normalized * speed;
            rb.velocity = dir;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Beam"))
        {
            ShipMovement.Instance.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            rb.velocity = new Vector3(0, 0, 0);
        }
    }





}
