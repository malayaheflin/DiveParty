using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapPositioner : MonoBehaviour
{
    [SerializeField] GameObject ship, mapParent;
    [SerializeField] LineRenderer lineRend;
    [SerializeField] Sprite shallow, mid, deep;
    private float xPos, yPos, shipYPos;
    private List<Vector3> linePoints;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = mapParent.GetComponent<SpriteRenderer>();
        linePoints = new List<Vector3>();
        lineRend.startWidth = 0.05f;
        lineRend.endWidth = 0.05f;
        StartCoroutine("UpdateTrail");
    }

    void Update()
    {
        shipYPos = ship.transform.position.y;

        // move ship icon
        xPos = Mathf.InverseLerp(-75f, 48f, ship.transform.position.x) * 2.4f - 1.2f;
        yPos = Mathf.InverseLerp(-90f, 19f, shipYPos) * 2.4f - 1.2f;
        transform.localPosition = new Vector3(xPos, yPos, 0);

        // update map BG -17, -53
        if (shipYPos < -53f)
            spriteRenderer.sprite = deep;
        else if (shipYPos < -17f)
            spriteRenderer.sprite = mid;
        else
            spriteRenderer.sprite = shallow;
    }
    
    IEnumerator UpdateTrail()
    {
        yield return new WaitForSeconds(0.5f);
        linePoints.Add(transform.localPosition);
        lineRend.positionCount++;
        while (true)
        {
            // var debugStr = "points: ";
            // foreach (Vector3 linePoint in linePoints)
            // {
            //     debugStr += linePoint + ", ";
            // }
            // print(debugStr);
            linePoints.Add(transform.localPosition);
            lineRend.positionCount++;
            lineRend.SetPositions(linePoints.ToArray());
            float width =  lineRend.startWidth;
            // float length = lineRend.length;
            lineRend.material.mainTextureScale = new Vector2(1 / width, 1.0f);
            yield return new WaitForSeconds(2f);
        }
    }
}
