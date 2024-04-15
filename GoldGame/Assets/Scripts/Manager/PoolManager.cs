using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public enum LoadAssetToPoolType
{
    Resources,
    Addressable,
    Other,

}
public class PoolManager : Singleton<PoolManager>
{
    Dictionary<string, PoolData> objectPool = new Dictionary<string, PoolData>();
    public Transform allPoolDataParent;
    private LoadAssetToPoolType loadType;
    protected override void Awake()
    {
        base.Awake();
        //allPoolDataParent = new GameObject("ObjPoolManager").transform;
    }

    public void ReleaseObjForPath(GameObject obj)
    {
        if (!objectPool.ContainsKey(obj.name))
        {
            CreatePoolData(allPoolDataParent, obj.name);
        }
        objectPool[obj.name].PushObj(obj);
    }

    //放入对象池
    public void ReleaseObj(GameObject obj)
    {
        if (!objectPool.ContainsKey(obj.name))
        {
            CreatePoolData(allPoolDataParent, obj);
        }
        objectPool[obj.name].PushObj(obj);
    }

    /// <summary>
    /// 通过预制体路径获取对象池内对象
    /// </summary>
    /// <param name="objPathName">预制体路径</param>
    /// <returns></returns>
    public GameObject GetObj(string objPathName)
    {
        //GameObject obj = null;
        if (!objectPool.ContainsKey(objPathName))
        {
            CreatePoolData(allPoolDataParent, objPathName);
        }
        return objectPool[objPathName].PullObjForPath();
    }

    /// <summary>
    /// 通过预制体直接赋值获取对象池内对象
    /// </summary>
    /// <param name="obj">预制体</param>
    /// <returns></returns>
    public GameObject GetObj(GameObject obj)
    {
        //GameObject obj = null;
        if (!objectPool.ContainsKey(obj.name))
        {
            CreatePoolData(allPoolDataParent,obj);
        }
        return objectPool[obj.name].PullObj();
    }

    //清空对象池
    public void ClearPool()
    {
        objectPool.Clear();
    }

    //创建小对象池 通过路径
    public void CreatePoolData(Transform allPoolDataParent,string objPathName)
    {
        objectPool.Add(objPathName, new PoolData(allPoolDataParent, loadType, objPathName));

    }

    //直接赋值预制体
    public void CreatePoolData(Transform allPoolDataParent, GameObject obj)
    {
        objectPool.Add(obj.name, new PoolData(allPoolDataParent, obj));

    }

    /// <summary>
    /// 获取预制体路径
    /// </summary>
    /// <param name="prefab">预制体</param>
    /// <returns></returns>
    //public string GetPrefabPath(GameObject prefab)
    //{
    //    string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefab);
    //    path = path.Substring(17, path.Length - 24);
    //    Debug.Log(path);
    //    return path;
    //}
}

/// <summary>
/// 小对象池
/// </summary>
public class PoolData
{
    public List<GameObject> pool = new List<GameObject>();
    public Transform poolParent;//大对象池
    private LoadAssetToPoolType loadType;
    private string objPathName;
    private GameObject item;

    public PoolData(Transform poolParent,LoadAssetToPoolType loadType,string objPathName)
    {
        this.poolParent = new GameObject(objPathName+"Parent").transform;//创建父物体
        this.poolParent.SetParent(poolParent);
        this.objPathName = objPathName;
        this.loadType = loadType;

    }

    public PoolData(Transform poolParent, GameObject item)
    {
        this.poolParent = new GameObject(item.name + "Parent").transform;//创建父物体
        this.poolParent.SetParent(poolParent);
        this.item = item;
    }

    /// <summary>
    /// 放入对象池
    /// </summary>
    /// <param name="item">对象物体</param>
    public void PushObj(GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(poolParent);
        pool.Add(item);
    }

    /// <summary>
    /// 取出对象
    /// </summary>
    /// <returns></returns>
    public GameObject PullObj()
    {
        GameObject obj;
        if (pool.Count > 0)
        {
            obj = pool[0];
            pool.RemoveAt(0);
            obj.SetActive(true);
        }
        else
        {
            //obj = GeneratorItem(loadType, objPathName);
            obj = GeneratorItem(item);
            //Debug.Log(objPathName);
            obj.name = item.name;
        }
        return obj;
    }

    /// <summary>
    /// 取出对象
    /// </summary>
    /// <returns></returns>
    public GameObject PullObjForPath()
    {
        GameObject obj;
        if (pool.Count > 0)
        {
            obj = pool[0];
            pool.RemoveAt(0);
            obj.SetActive(true);
        }
        else
        {
            obj = GeneratorItem(loadType, objPathName);
            //obj = GeneratorItem(item);
            //Debug.Log(objPathName);
            obj.name = objPathName;
        }
        return obj;
    }

    /// <summary>
    /// 通过预制体地址生成对象
    /// </summary>
    /// <param name="loadType">加载资源方式</param>
    /// <param name="objPathName">资源地址</param>
    /// <returns></returns>
    public GameObject GeneratorItem(LoadAssetToPoolType loadType,string objPathName)
    {
        GameObject obj = null;
        switch (loadType)
        {
            case LoadAssetToPoolType.Resources:
                obj = GameObject.Instantiate(Resources.Load<GameObject>(objPathName), poolParent);
                break;
            case LoadAssetToPoolType.Addressable:
                break;
        }
        return obj;
    }

    /// <summary>
    /// 通过预制体对象生成对象
    /// </summary>
    /// <param name="loadType"></param>
    /// <param name="objPathName"></param>
    /// <returns></returns>
    public GameObject GeneratorItem(GameObject obj)
    {
        //GameObject obj = null;
        //switch (loadType)
        //{
        //    case LoadAssetToPoolType.Resources:
        //        obj = GameObject.Instantiate(Resources.Load<GameObject>(objPathName), poolParent);
        //        break;
        //    case LoadAssetToPoolType.Addressable:
        //        break;
        //}
        return GameObject.Instantiate(obj, poolParent); ;
    }
}
