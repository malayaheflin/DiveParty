using ETC.Platforms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tester : MonoBehaviour
{
    public int num;
    public Image spriteRenderer;
    public JamoDrum drum;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<Image>();
        //drum.AddHitEvent(onHit);
        //drum.AddReleaseEvent(onRelease);
    }

    private void Update()
    {
        if (drum.release[num] == 1)
        {
            Debug.Log($"{num} released");
        }
    }

    void onHit(int which)
    {
        if(which == num)
        {
            spriteRenderer.enabled = true;
        }
    }

    void onRelease(int which)
    {
        if(which == num)
        {
            spriteRenderer.enabled = false;
        }
    }
}
