using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light : MonoBehaviour, Tool
{
    [SerializeField] GameObject stunBoundary, lightSpriteMap, ship;
    [SerializeField] Light2D lightObj;
    [SerializeField] float stunCooldownTime;
    private bool cooldown = false;

    void Update()
    {
        float newGval = Mathf.InverseLerp(-90f, 19f, ship.transform.position.y) * 200 + 54;
        lightObj.color = new Color(236, newGval, 255);
    }

    public void ToolAction() // stun!
    {
        if (!cooldown)
        {
            cooldown = true;
            Debug.Log("trigger stun");
            StartCoroutine("TriggerStun");
        }
    }

    IEnumerator TriggerStun()
    {
        stunBoundary.SetActive(true);
        lightObj.intensity = 0.1f;
        yield return new WaitForSeconds(0.2f); // time that light is bright
        stunBoundary.SetActive(false);
        lightObj.intensity = 0f;
        lightSpriteMap.SetActive(false);
        StartCoroutine("RestoreLight");
        yield return new WaitForSeconds(stunCooldownTime); // cooldown
        cooldown = false;
    }

    IEnumerator RestoreLight()
    {
        float increaseBy = 0.01f / (stunCooldownTime * 10);
        while (lightObj.intensity < 0.01) // hard coded for 1.5 default light val
        {
            lightObj.intensity += increaseBy;
            yield return new WaitForSeconds(0.1f);
        }
        lightSpriteMap.SetActive(true);
    }
}
