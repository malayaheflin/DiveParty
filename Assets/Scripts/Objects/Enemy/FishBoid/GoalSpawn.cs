using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSpawn : MonoBehaviour
{
    public bool goalActive = false;
    public Vector2 goalLocation = new Vector2(0, 0);

    public float radius = 5f;
    public float generateBreak = 3f;

    private Vector2 originPos;
    private bool temp;

    // Start is called before the first frame update
    void Start()
    {
        originPos = new Vector2(this.transform.position.x, this.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(GenerateGoal());
    }

    public void GenerateGoalPos()
    {
       goalLocation = originPos + Random.insideUnitCircle * radius;
    }

    public IEnumerator GenerateGoal()
    {
        if (!temp)
        {
            temp = true;
            goalActive = true;
            GenerateGoalPos();
            yield return new WaitForSeconds(2f);
            goalActive = false;
            yield return new WaitForSeconds(generateBreak - 2);
            temp = false;
        }
    }
}
