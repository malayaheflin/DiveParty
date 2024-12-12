using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : SingletonMono<GameMgr>
{

    [Header("DataRecord")]
    public E_GameState state;

    public float depth;
    public float depthStart;
    public int money;
    public float timeLimit = 120;
    public float restOfTime;

    public float timeAlarm = 30;


    //Treasure Tracking

    public float treasurePileRate = 100; 
    private float treasurePileTracker;
    private int treasureIndex = 0;
    private int treasureMax = 8;

    public GameObject treasurePiles;

    [Header("RankData")]
    public RankData_SO rankData;

    [SerializeField]
    private GamePanel gamePanel;
    [SerializeField]
    private GameObject beginPanel;

    [Header("HideLight")]
    public GameObject light2D_SpotLight;
    public GameObject light2D_PointLight;

    private bool isPlayingAlarm;

    private void Start()
    {
        restOfTime = timeLimit;
        depth = this.transform.position.y;
        gamePanel.gameObject.SetActive(false);
        treasurePileTracker = treasurePileRate;

    }

    private void Update()
    {
        if(state == E_GameState.GameBegin)
        {
            CaculateTime();
        }
        else if (state == E_GameState.GameCanRestart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    
    public void ChangeGameStateToBegin()
    {
        SoundMgr.Instance.PlaySound("ui_game_start");
        SoundMgr.Instance.PlayMusic("music_bgm");
        state = E_GameState.GameBegin;
        gamePanel.gameObject.SetActive(true);
        beginPanel.SetActive(false);
        light2D_SpotLight.SetActive(false);
        light2D_PointLight.SetActive(false);
    }

    public void RestartGame()
    {
        // Reload the scene
        SceneManager.LoadScene("MalayaNinaScene");
    }

    public void CaculateTime()
    {
        restOfTime = (timeLimit -= Time.deltaTime);
        if(restOfTime <= timeAlarm)
        {
            StartCoroutine(PlayAlarm());
        }
        if(restOfTime <= 0)
        {
            state = E_GameState.GameEnd;

            GamePanel.Instance.gameObject.SetActive(false);

            // Call UI to appear
            RankPanel.Instance.writeNamePart.gameObject.SetActive(true);
            RankPanel.Instance.txtScore.text = money.ToString() + " $";
        }
    }

    public void RecordTheDepth()
    {
        depth = ShipMovement.Instance.transform.position.y;
        //depth = nowDepth < depth ? nowDepth : depth;
    }

    public void AddMoney(int money)
    {
        this.money += money;
        treasurePileTracker -= money;

        this.money = this.money < 0 ? 0 : this.money;


        // Refresh UI
        gamePanel.RefreshMoney(this.money);
        Debug.Log($"Current Treasure: {this.money}");

        //Set Treasure Pile Active in Ship UI
        if (treasurePileTracker <= 0 && treasureIndex < treasureMax){
            setTreasurePileActive();
            treasurePileTracker = treasurePileRate;
        }

    }

    public void setTreasurePileActive(){
        treasurePiles.transform.GetChild(treasureIndex).gameObject.SetActive(true);
        Debug.Log("Treasure Added");
        treasureIndex++;
    }

    public void setTreasurePileInactive(float moneyLost){

        treasurePileTracker += moneyLost;

        if (treasurePileTracker >= treasurePileRate && treasureIndex >= 1){
            treasurePiles.transform.GetChild(treasureIndex).gameObject.SetActive(false);
            Debug.Log("Treasure Lost");
            treasurePileTracker = treasurePileRate / 2;
            treasureIndex--;

        }


    }

    public void SaveTeamNameAndPoints(string teamName)
    {
        rankData.SaveNewTeamScore(teamName, money);
    }

    IEnumerator PlayAlarm()
    {
        if (!isPlayingAlarm)
        {
            print("AAAAAAAAAAAA");
            isPlayingAlarm = true;
            float breakTime = restOfTime / timeAlarm;
            breakTime = breakTime < 0.25f ? 0.25f : breakTime;

            SoundMgr.Instance.PlaySound("ui_oxygen_beeps");
            yield return new WaitForSeconds(breakTime);

            isPlayingAlarm = false;

        }
    }
}
