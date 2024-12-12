using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : SingletonMono<GamePanel>
{
    [SerializeField]
    private TextMeshPro txtTime;
    [SerializeField]
    private TextMeshPro txtMoney;

    private void Start()
    {
        txtTime.text = "Time: \n" + GameMgr.Instance.restOfTime.ToString();
        txtMoney.text = "0 $";
    }

    private void Update()
    {
        if(GameMgr.Instance.state == E_GameState.GameBegin)
        {
            float t = Mathf.Round(GameMgr.Instance.restOfTime * 10.0f) * 0.1f ;
            txtTime.text = "Time: \n" + t.ToString();
        }
        RefreshDepth();
    }


    public void RefreshDepth()
    {
        float d = Mathf.Round(GameMgr.Instance.depth * 10.0f) * 0.1f ;
    }

    public void RefreshMoney(int money)
    {
        txtMoney.text = money.ToString()  + " $";
    }
}
