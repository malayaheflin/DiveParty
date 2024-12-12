using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMgr : SingletonMono<ResourceMgr>
{
    public PrefabsData_SO prefabsDataList;

    public GameObject LoadObj(string name)
    {
        return prefabsDataList.GetPrefabFromName(name);
    }
}
