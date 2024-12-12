using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Attract : SingletonMono<Attract>, Tool
{
    [SerializeField] GameObject beam, magnetSprite;
    [SerializeField] Light2D detectorLight;
    [SerializeField] Animator magnetAnim;
    public static List<BaseTreasure> allTreasure = new List<BaseTreasure>();
    private bool isAttracting = false;
    
    void Start()
    {
        magnetAnim.SetBool("AttractOn", isAttracting);
    }
    void Update()
    {
        if (GameMgr.Instance.state == E_GameState.GameCanRestart)
        {
            allTreasure.Clear();
            return;
        }
        // find closest treasure
        Vector3 magnetPos = this.transform.position;
        float minDist = float.PositiveInfinity;
        BaseTreasure closestTreasure = null;
        foreach (BaseTreasure treasure in allTreasure)
        {
            var dist = (magnetPos - treasure.transform.position).sqrMagnitude;
            if (dist < minDist)
            {
                closestTreasure = treasure;
                minDist = dist;
            }
        }
        Vector3 targetDir = closestTreasure.transform.position - transform.position;
        Vector3 magnetDir = magnetSprite.transform.position - transform.position;
        float dotProd = Vector3.Dot(targetDir.normalized, magnetDir.normalized); // -1 = opposites, 1 = same
        detectorLight.intensity = Mathf.Lerp(-1f, 1f, dotProd) + 0.5f;
    }

    public void ToolAction()
    {
        // if (isAttracting)
        //     TurnOff();
        // else
        //     TurnOn();

        // isAttracting = !isAttracting;

        beam.SetActive(true);
        SoundMgr.Instance.PlaySound("sfx_attract_beam");
        magnetAnim.SetBool("AttractOn", true);
    }



    public void TurnOff()
    {
        beam.SetActive(false);
        magnetAnim.SetBool("AttractOn", false);
    }

    
}
