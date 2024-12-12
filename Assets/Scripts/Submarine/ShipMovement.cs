using System.Collections;
using System.Collections.Generic;
using ETC.Platforms;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShipMovement : SingletonMono<ShipMovement>
{
    /*  CONTROLS FOR COMPUTER TESTING:
     *                      L rotate        R rotate       hit
     * pilot: player 1          A              D            S
     * light: player 2          7              9            8
     * attack: player 3         4              6            5
     * attract: player 4        1              3            2
     *
    */


    // jamo drum control
    private JamoDrum jamoDrum;
    public event HitEventHandler hitEvent;
    public event SpinEventHandler spinEvent;
    public event HitEventHandler releaseEvent;
    private bool onStartScreen = true;
    
    // tool game objects
    [SerializeField] GameObject pilotTool, lightTool, attackTool, attractTool, startShip, catIcon, bearIcon, birdIcon, dogIcon;
    private GameObject[] icons = new GameObject[4];
    private GameObject[] tools = new GameObject[4];
    private int playersReady = 0;
    private int playersReadyRestart = 0;

    // ship movement
    [SerializeField] float speed;
    private bool isMoving = false;

    // light
    [SerializeField] Light2D globalLight;

    private bool isPlayingLightSound;
    private bool isPlayingMoveSound;
    public float scrambleAttackTime = 5f;

    private void Start()
    {
        jamoDrum = GetComponent<JamoDrum>();
        if (jamoDrum == null)
        {
            Debug.LogError("JamoKey script has to be attached on the object with JamoDrum object!");
        }
        hitEvent = Hit;
        spinEvent = Spin;
        releaseEvent = Release;
        jamoDrum.AddHitEvent(hitEvent);
        jamoDrum.AddSpinEvent(spinEvent);
        jamoDrum.AddReleaseEvent(releaseEvent);

        // assigning tools to players
        tools[0] = attackTool;
        tools[1] = attractTool;
        tools[2] = pilotTool;
        tools[3] = lightTool;

        // assigning icons
        icons[0] = dogIcon;
        icons[1] = bearIcon;
        icons[2] = catIcon;
        icons[3] = birdIcon;

        foreach (GameObject tool in tools)
        {
            Debug.Log(tool);
        }

    }

    void Update()
    {
        if (onStartScreen && playersReady == 4)
        {
            GameMgr.Instance.ChangeGameStateToBegin();
            startShip.SetActive(false);
            onStartScreen = false;
            isMoving = true;
        }
        else if (isMoving && !onStartScreen)
        {
            if (GameMgr.Instance.state == E_GameState.GameBegin)
            {
                transform.position += pilotTool.transform.up * speed * Time.deltaTime;
                StartCoroutine(PlayMoveSoundRotate());
            }
        }
        else if (playersReadyRestart == 4 && GameMgr.Instance.state == E_GameState.GameCanRestart)
        {
            GameMgr.Instance.RestartGame();
        }
        // change BG light to match depth (mapping 6 to -80 y axis to 0.03 to 0.005 brightness)
        globalLight.intensity = Mathf.InverseLerp(-80f, 6f, transform.position.y) * 0.25f + 0.005f; 
    }

    void Hit(int playerID)
    {
        Debug.Log("hit by player " + playerID);

        switch (GameMgr.Instance.state)
        {
            case E_GameState.GameMenu:
                ReadyUp(playerID);
                SoundMgr.Instance.PlaySound("ui_player_ready");
                break;
            case E_GameState.GameBegin:
                if (playerID == 3)
                {
                    Debug.Log("stop moving");
                    isMoving = false;
                    // isMoving = !isMoving;
                }
                else
                {
                    tools[playerID - 1].GetComponent<Tool>()?.ToolAction();
                }
                break;
            case E_GameState.GameEnd:
                RankPanel.Instance.SelectAlphabet(playerID);
                break;
            case E_GameState.GameCanRestart:
                playersReadyRestart++;
                break;
        }
    }

    void ReadyUp(int playerID)
    {
        bool isActive = !icons[playerID - 1].activeSelf;
        icons[playerID - 1].SetActive(isActive);
        if (isActive)
            playersReady++;
        else
            playersReady--;
    }

    void Spin(int playerID, int delta)
    {
        if (GameMgr.Instance.state == E_GameState.GameBegin)
        {
            Debug.Log("spin by player " + playerID + " w/ value of " + delta);

            // select correct tool & rotate it
            GameObject tool = tools[playerID - 1];
            tool.transform.Rotate(0f, 0f, delta * 3.5f);

            if(playerID == 4)
            {
                StartCoroutine(PlayLightSoundRotate());
            }
        }
        else if(GameMgr.Instance.state == E_GameState.GameEnd)
        {
            RankPanel.Instance.SwitchAlphabetOfPlayer(playerID, delta);
        }
    }

    public void GetAttacked()
    {
        Debug.Log("ship attacked");
        SoundMgr.Instance.PlaySound("sfx_ship_damage");
        GameMgr.Instance.AddMoney(-5);
    }

    void Release(int playerID)
    {
        if (GameMgr.Instance.state == E_GameState.GameCanRestart)
        {
            playersReadyRestart--;
        }
        else if (playerID == 3)
        {
            Debug.Log("released");
            isMoving = true;
        }
        else if (playerID == 2)
        {
            Attract.Instance.TurnOff();
        }
    }

    public void TempScrambleCaller()
    {
        StartCoroutine("TempScramble");
    }

    private IEnumerator TempScramble()
    {
        int rand = Random.Range(1, 4);
        switch (rand)
        {
            case 1: // rotate 90
                GamePanel.Instance.transform.rotation = Quaternion.Euler(0, 0, 90);
                tools[3] = attackTool;
                tools[0] = attractTool;
                tools[1] = pilotTool;
                tools[2] = lightTool;
                break;
            case 2: // rotate 180
                GamePanel.Instance.transform.rotation = Quaternion.Euler(0, 0, 180);
                tools[2] = attackTool;
                tools[3] = attractTool;
                tools[0] = pilotTool;
                tools[1] = lightTool;
                break;
            case 3: // rotate 270
                GamePanel.Instance.transform.rotation = Quaternion.Euler(0, 0, 270);
                tools[1] = attackTool;
                tools[2] = attractTool;
                tools[3] = pilotTool;
                tools[0] = lightTool;
                break;
        }
        yield return new WaitForSeconds(scrambleAttackTime); // reset
        GamePanel.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        tools[0] = attackTool;
        tools[1] = attractTool;
        tools[2] = pilotTool;
        tools[3] = lightTool;
    }

    IEnumerator PlayLightSoundRotate()
    {
        if (!isPlayingLightSound)
        {
            isPlayingLightSound = true;
            SoundMgr.Instance.PlaySound("sfx_move_tool");
            yield return new WaitForSeconds(1f);
            isPlayingLightSound = false;
        }
    }

    IEnumerator PlayMoveSoundRotate()
    {
        if (!isPlayingMoveSound)
        {
            isPlayingMoveSound = true;
            SoundMgr.Instance.PlaySound("sfx_ship_movement");
            yield return new WaitForSeconds(10f);
            isPlayingMoveSound = false;
        }
    }
}
