using TMPro;
using UnityEngine;

public class APieceOfRank : MonoBehaviour
{
    public TextMeshProUGUI txtTeamName;
    public TextMeshProUGUI txtMoney;


    public void RefreshData(string teamName, int money)
    {
        txtTeamName.text = teamName;
        txtMoney.text = money.ToString() + "$";
    }
}
