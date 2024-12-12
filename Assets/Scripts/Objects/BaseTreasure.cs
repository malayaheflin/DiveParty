using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseTreasure : MonoBehaviour
{
    private float speed = 1.5f;
    [SerializeField]
    private int money = 1;
    private bool isInBeam = false;
    public bool isCreatureDrop;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Awake()
    {
        if (!isCreatureDrop)
            Attract.allTreasure.Add(this);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("beamed");
        if (other.CompareTag("Beam"))
        {
            isInBeam = true;
            // Vector3 directionToSpaceship = ShipMovement.Instance.transform.position - transform.position;
            Vector3 dir = (ShipMovement.Instance.transform.position - transform.position).normalized * speed;
            rb.velocity = dir;
            //transform.Translate(directionToSpaceship * Time.deltaTime * speed);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Beam"))
        {
            isInBeam = false;
            rb.velocity = new Vector3(0, 0, 0);
            Debug.Log("not in beam");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Submarine") && isInBeam)
        {
            Debug.Log("got treasure");
            SoundMgr.Instance.PlaySound("sfx_collect_treasure");
            if (!isCreatureDrop)
                Attract.allTreasure.Remove(this);
            Destroy(gameObject);
            // increase treasure points here
            GameMgr.Instance.AddMoney(money);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Submarine") && isInBeam)
        {
            Debug.Log("got treasure");
            if (!isCreatureDrop)
                Attract.allTreasure.Remove(this);
            Destroy(gameObject);
            // increase treasure points here
            GameMgr.Instance.AddMoney(money);


        }
    }

}
