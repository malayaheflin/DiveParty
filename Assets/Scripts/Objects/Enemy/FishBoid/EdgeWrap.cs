using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeWrap : MonoBehaviour
{
    public float radius;

    private Pilot pilot;
    private float xOrigin;
    private float yOrigin;

    private void Start()
    {
        yOrigin = this.transform.position.y;
        xOrigin = this.transform.position.x;
        pilot = this.GetComponent<Pilot>();
    }

    void Update()
    {
        /*if (transform.position.y <= yOrigin - radius)
        {
            transform.position = new Vector2(transform.position.x, yOrigin - radius);
            pilot.targetVelocity = Vector2.right;
        }
        else if (transform.position.y >= this.transform.position.y + radius)
        {
            transform.position = new Vector2(transform.position.x, yOrigin + radius);
            pilot.targetVelocity = Vector2.right;
        }

        if (transform.position.x <= xOrigin - radius)
        {
            transform.position = new Vector2(xOrigin - radius, transform.position.y);
            pilot.targetVelocity = Vector2.right;
        }
        else if (transform.position.x >= xOrigin + radius)
        {
            transform.position = new Vector2(xOrigin + radius, transform.position.y);
            pilot.targetVelocity = Vector2.right;
        }*/
    }
}
