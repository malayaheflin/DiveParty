using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PoolMgr : SingletonMono<PoolMgr>
{
    //Pool closet
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    private GameObject poolObj;

    /// <summary>
    /// Take Out OBj From Pool
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetObj(string name)
    {
        // have drawer and have items in it
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
            return poolDic[name].GetObj();
        else
        {
            GameObject obj = GameObject.Instantiate(ResourceMgr.Instance.LoadObj(name));
            //change object's name as drawer
            obj.name = name;
            return obj;
        }
    }
    public GameObject GetObj(string name, Vector3 pos)
    {
        //have drawer and have items in it
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            GameObject obj = poolDic[name].GetObj();
            obj.transform.position = pos;
            return obj;
        }
        else
        {
            GameObject obj = GameObject.Instantiate(ResourceMgr.Instance.LoadObj(name));
            //Change new Object Name as the same
            obj.name = name;
            obj.transform.position = pos;
            return obj;
        }
    }
    public GameObject GetObj(string name, Vector3 pos, Quaternion rotation)
    {
        //have drawer and have items in it
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            GameObject obj = poolDic[name].GetObj();
            obj.transform.position = pos;
            obj.transform.rotation = rotation;
            return obj;
        }
        else
        {

            GameObject obj = GameObject.Instantiate(ResourceMgr.Instance.LoadObj(name));
            //Change new Object Name as the same
            obj.name = name;
            obj.transform.position = pos;
            obj.transform.rotation = rotation;
            return obj;
        }
    }

    /// <summary>
    /// give me the useless thing
    /// </summary>
    public void PushObj(string name, GameObject obj)
    {
        if (poolObj == null)
            poolObj = new GameObject("Pool");

        //has drawer
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        //no drawer
        else
        {
            poolDic.Add(name, new PoolData(obj, poolObj));
        }
    }


    /// <summary>
    /// Clear all the drawer
    /// Use in changing scene
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}

/// <summary>
/// drawer data(a type of container)
/// </summary>
public class PoolData
{
    //all drawers' father
    public GameObject fatherObj;
    //the container of objects
    public List<GameObject> poolList;

    public PoolData(GameObject obj, GameObject poolObj)
    {
        //create a drawer and then its father  then make it son obj of the poolMgr
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject>() { };
        PushObj(obj);
    }

    /// <summary>
    /// push object in drawer
    /// </summary>
    /// <param name="obj"></param>
    public void PushObj(GameObject obj)
    {
        //hide
        obj.SetActive(false);
        //save
        poolList.Add(obj);
        //setFather
        obj.transform.parent = fatherObj.transform;
    }

    /// <summary>
    /// get object from drawer
    /// </summary>
    /// <returns></returns>
    public GameObject GetObj()
    {
        GameObject obj = null;
        // get the first one
        obj = poolList[0];
        poolList.RemoveAt(0);
        //setActive
        obj.SetActive(true);
        // DEpart father and son
        obj.transform.parent = null;

        return obj;
    }
}

