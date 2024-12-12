using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSeeking : MonoBehaviour
{
    private Vector2 targetVelocity;
    private Pilot pilot;

    [SerializeField]
    private GoalSpawn goalSpawn;

    void Start()
    {
        pilot = GetComponentInParent<Pilot>();
    }

    void Update()
    {
        if (goalSpawn.goalActive == true && Vector2.Distance(goalSpawn.goalLocation, transform.position) > 1)
        {

            targetVelocity = goalSpawn.goalLocation - new Vector2(transform.position.x, transform.position.y);
            pilot.goalVelocity = targetVelocity.normalized * 3;
        }

        if (goalSpawn.goalActive == false)
        {
            targetVelocity = new Vector2(0, 0);
            pilot.goalVelocity = targetVelocity;
        }

        
    }
}
