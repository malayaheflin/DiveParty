using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[Serializable]
public class TextInfo
{
    public bool isLocked;
    public int alphaIndex;
}

public class RankPanel : SingletonMono<RankPanel>
{
    [Header("WriteNamePart")]
    public GameObject writeNamePart;

    public TextMeshProUGUI txtScore;
    public TextMeshProUGUI[] txtTeamNameList;
    public TextMeshProUGUI[] txtPlayersAlphabet;

    public List<string> alphas_26;

    public List<TextInfo> playerAlphaBetInfo;


    [Header("RankPart")]
    public GameObject rankPart;

    public List<APieceOfRank> rankUIList;
    public TextMeshProUGUI[] txtTeamNameTop3;
    public TextMeshProUGUI[] txtTeamGoalTop3;

    private bool hasSubmitTeamName;

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// ChangeAlphabet
    /// </summary>
    /// <param name="playerIndex"> 0 for player 1 // 3 for player 4</param>
    public void SwitchAlphabetOfPlayer(int playerIndex, int delta)
    {
        if (playerAlphaBetInfo[playerIndex - 1].isLocked)
            return;
        playerAlphaBetInfo[playerIndex - 1].alphaIndex += delta;
        
        if(playerAlphaBetInfo[playerIndex - 1].alphaIndex > 25)
        {
            playerAlphaBetInfo[playerIndex - 1].alphaIndex = 0;
        }
        else if(playerAlphaBetInfo[playerIndex - 1].alphaIndex < 0)
        {
            playerAlphaBetInfo[playerIndex - 1].alphaIndex = 25;
        }

        SoundMgr.Instance.PlaySound("ui_text_scroll");
        txtPlayersAlphabet[playerIndex - 1].text = alphas_26[playerAlphaBetInfo[playerIndex - 1].alphaIndex];
        txtTeamNameList[playerIndex - 1].text = txtPlayersAlphabet[playerIndex - 1].text;
    }

    public void SelectAlphabet(int playerIndex)
    {
        SoundMgr.Instance.PlaySound("ui_text_select");
        Color temp = txtTeamNameList[playerIndex - 1].color;
        txtTeamNameList[playerIndex - 1].color = new Color(temp.r, temp.g, temp.b, 1);
        playerAlphaBetInfo[playerIndex - 1].isLocked = true;
        CheckEveryonePickName();
    }

    public void CheckEveryonePickName()
    {
        if (hasSubmitTeamName)
            return;
        bool b = true;
        for (int i = 0; i < playerAlphaBetInfo.Count; i++)
        {
            b = playerAlphaBetInfo[i].isLocked && b;
        }
        if (b)
        {
            SaveTeamNameAndScore();
            hasSubmitTeamName = true;
            writeNamePart.SetActive(false);
            rankPart.SetActive(true);

            RefreshTop3TeamName();
            RefreshRecent5TeamName();

            GameMgr.Instance.state = E_GameState.GameCanRestart;
        }
    }

    public void SaveTeamNameAndScore()
    {
        string str = "";
        foreach (TextMeshProUGUI i in txtTeamNameList)
        {
            str += i.text;
        }
        GameMgr.Instance.SaveTeamNameAndPoints(str);
    }

    public void RefreshTop3TeamName()
    {
        for (int i = 0; i < 3; i++)
        {
            if (GameMgr.Instance.rankData.rankTop3List[i].teamName.Equals(""))
                return;
            txtTeamNameTop3[i].text = GameMgr.Instance.rankData.rankTop3List[i].teamName;
            txtTeamGoalTop3[i].text = GameMgr.Instance.rankData.rankTop3List[i].money.ToString();
        }
    }

    public void RefreshRecent5TeamName()
    {
        RankDetail temp;
        for (int i = 0; i < 5; i++)
        {
            temp = GameMgr.Instance.rankData.rankRecent5List[i];
            rankUIList[i].RefreshData(temp.teamName, temp.money);
        }
    }

    public void Init()
    {
        foreach (var text in txtTeamNameList)
        {
            text.text = "A";
            text.color = new Color(1, 1, 1, 0.5f);
        }

        foreach (var text in txtPlayersAlphabet)
        {
            text.text = "A";
        }

        foreach (var text in txtTeamNameTop3)
        {
            text.text = "";
        }
        foreach (var text in txtTeamGoalTop3)
        {
            text.text = "";
        }

        rankPart.SetActive(false);
        writeNamePart.SetActive(false);

        
    }
}
