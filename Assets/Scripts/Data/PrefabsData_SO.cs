using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabsData_SO",
menuName = "PrefabsData_SO/ PrefabsData_SO")]
public class PrefabsData_SO : ScriptableObject
{
    public List<PrefabsDetails> prefabsDetailsList;
    public GameObject GetPrefabFromName(string name)
    {
        return prefabsDetailsList.Find(x => x.name == name).obj;
    }
}

[System.Serializable]
public class PrefabsDetails
{
    public string name;
    public GameObject obj;
}
