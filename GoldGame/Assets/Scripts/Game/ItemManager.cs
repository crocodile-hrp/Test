using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public float createTime = 0;
    public float createLiftItemTime = 5;
    public GameObject[] ItemPrefabs;
    [Header("冲过来的敌人")] public GameObject atkEnemy;//待做
    public bool islandScape;
    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isGameOver)
            return;
        createPrefab();
    }

    void createPrefab()
    {
        createTime -= Time.deltaTime;
 
        if(createTime <= 0)
        {
            float random = Random.Range(0, 1.1f);
            int prefabIndex;
            if (random > 0.6f)
                prefabIndex = 0;
            else if (random > 0.2f)
                prefabIndex = 1;
            else
                prefabIndex = 2;
            GameObject prefab = PoolManager.Instance.GetObj(ItemPrefabs[prefabIndex]);
            prefab.transform.position = RandomCreatPos();
            if (GameManager.Instance.gameTime <= 20)
                createTime = Random.Range(0.5f, 1.1f);
            else if (GameManager.Instance.gameTime <= 60)
                createTime = Random.Range(0.2f, 1f);
            else
                createTime = Random.Range(0, 0.5f);
        }
        CreateAddLiftPrefab();

    }

    void CreateAddLiftPrefab()
    {
        if (GameManager.Instance.lift < 3)
        {
            createLiftItemTime -= Time.deltaTime;
            if (createLiftItemTime <= 0)
            {
                GameObject prefab = PoolManager.Instance.GetObj(ItemPrefabs[3]);
                prefab.transform.position = RandomCreatPos();
                createLiftItemTime = Random.Range(10, 45f);
            }
        }
    }

    public Vector3 RandomCreatPos()
    {
        int randomX = Random.Range(0, Screen.width);
        return Camera.main.ScreenToWorldPoint(new Vector3(randomX, Screen.height, 100));
    }
}
