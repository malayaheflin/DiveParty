using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RankData_SO",
menuName = "RankData_SO/ RankData_SO")]
public class RankData_SO : ScriptableObject
{
    public List<RankDetail> rankTop3List;
    public List<RankDetail> rankRecent5List;    // 4 is the newest data , 0 is the oldest data

    public void SaveNewTeamScore(string teamName, int money)
    {
        RankDetail newData = new RankDetail(teamName, money);
        // recent 5
        rankRecent5List.Add(newData);
        rankRecent5List.RemoveAt(0);
        // top3
        rankTop3List.Add(newData);
        rankTop3List.Sort((a, b) => b.money.CompareTo(a.money));
        rankTop3List.RemoveAt(3);
    }
    
}

[Serializable]
public class RankDetail
{
    public string teamName;
    public int money;

    public RankDetail(string name, int money)
    {
        teamName = name;
        this.money = money;
    }
}

