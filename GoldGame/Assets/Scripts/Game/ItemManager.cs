using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [Tooltip("创建基本道具间隔")] public float createTime = 0;
    [Tooltip("创建生命道具间隔")]public float createLifeItemTime = 5;
    [Tooltip("创建护盾道具间隔")] public float createShieldTime = 20;
    [Tooltip("Atk道具间隔")] public float createAtkTime = 30;

    /// <summary>
    /// 最大时间间隔
    /// </summary>
    [Header("最大时间间隔")] public float maxInterval = 1.2f;
    /// <summary>
    /// 最小时间间隔
    /// </summary>
    [Header("最小时间间隔")] public float minInterval = 0.2f;
    /// <summary>
    /// 时间间隔随机范围
    /// </summary>
    [Header("时间间隔随机范围")] public float randomRange = 0.1f;
    /// <summary>
    /// 到最小时间间隔所需时间
    /// </summary>
    [Header("到最小时间间隔所需时间")] public float maxToMinTime = 30f;

    public GameObject[] ItemPrefabs;
    [Tooltip("护盾道具")] public GameObject shieldItem;
    [Tooltip("ATK道具")] public GameObject atkItem;
    [Header("冲过来的敌人")] public GameObject atkEnemy;//待做

    [Header("是否在时停")]public bool isPause;
    [Header("时停持续时间")] public float pauseTime = 2f;
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
        if (GameManager.Instance.isGameOver || GameManager.Instance.bossIsDead)
            return;

        if(!isPause)
            createPrefab();

        //测试技能
        if (Input.GetKeyDown(KeyCode.L) || (GameManager.Instance.hitBossCount%9==0 && GameManager.Instance.hitBossCount>0))
        {
            TimePauseSkill();
        }
    }

    /// <summary>
    /// 生成预制体
    /// </summary>
    void createPrefab()
    {
        CreateBaseItem();
        CreateAddLiftPrefab();
        CreateBuffItem();

    }

    /// <summary>
    /// 生成基础物品 金币、盗贼、炸弹///后面这个炸弹改成陨石 不掉血 但是 会眩晕主角 或者减速主角？
    /// </summary>
    void CreateBaseItem()
    {
        createTime -= Time.deltaTime;

        if (createTime <= 0)
        {
            //float random = Random.Range(0, 1.1f);
            float random = Random.value;
            int prefabIndex;
            if (random >= 0.3f)
                prefabIndex = 0;
            else /*if (random >= 0.3f)*/
                prefabIndex = 1;
            //else
            //    prefabIndex = 2;  

            GameObject prefab = PoolManager.Instance.GetObj(ItemPrefabs[prefabIndex]);
            prefab.transform.position = RandomCreatPos();
            //if (GameManager.Instance.gameTime <= 20)
            //    createTime = Random.Range(0.5f, 1.1f);
            //else if (GameManager.Instance.gameTime <= 60)
            //    createTime = Random.Range(0.2f, 1f);
            //else
            //    createTime = Random.Range(0, 0.5f);

            //新
            if(GameManager.Instance.gameTime <= maxToMinTime)
            {
                createTime = Mathf.Lerp(maxInterval, minInterval, Mathf.Clamp01(GameManager.Instance.gameTime / maxToMinTime)) + Random.Range(-randomRange, randomRange);
            }
            else
            {
                createTime = minInterval + Random.Range(-randomRange, randomRange);
            }
        }
    }

    /// <summary>
    /// 生成生命道具
    /// </summary>
    void CreateAddLiftPrefab()
    {
        if (GameManager.Instance.life < GameManager.Instance.maxlife)
        {
            createLifeItemTime -= Time.deltaTime;
            if (createLifeItemTime <= 0)
            {
                GameObject prefab = PoolManager.Instance.GetObj(ItemPrefabs[3]);
                prefab.transform.position = RandomCreatPos();
                createLifeItemTime = Random.Range(10, 45f);
            }
        }
    }

    /// <summary>
    /// 生成Buff道具 （后期各种道具可以一起套用 到时间生成随机一个道具？）
    /// </summary>
    void CreateBuffItem()
    {
        createShieldTime -= Time.deltaTime;
        createAtkTime -= Time.deltaTime;
        if (createShieldTime <= 0)
        {
            GameObject prefab = PoolManager.Instance.GetObj(shieldItem);
            prefab.transform.position = RandomCreatPos();
            if(GameManager.Instance.gameTime <=60f)
                createShieldTime = Random.Range(10, 30f);
            else
                createShieldTime = Random.Range(10, 20f);
        }

        if(createAtkTime <= 0)
        {
            GameObject prefab = PoolManager.Instance.GetObj(atkItem);
            prefab.transform.position = RandomCreatPos();
            if (GameManager.Instance.gameTime <= 60f)
                createAtkTime = Random.Range(15, 30f);
            else
                createAtkTime = Random.Range(5, 30f);
        }


    }

    public Vector3 RandomCreatPos()
    {
        int randomX = Random.Range(0, Screen.width);
        return Camera.main.ScreenToWorldPoint(new Vector3(randomX, Screen.height, 100));
    }



    /// <summary>
    /// 时停技能
    /// </summary>
    public void TimePauseSkill()
    {
        isPause = true;
        Invoke("PauseSkillEnd", pauseTime);
    }

    void PauseSkillEnd()
    {
        isPause = false;
    }

    /// <summary>
    /// 反击子弹
    /// </summary>
    /// <param name="go"></param>
    public void BeatBack(GameObject boom ,GameObject boss)
    {
        Vector2 dir = (Vector2)boss.transform.position - (Vector2)boom.transform.position;
        boom.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        boom.GetComponent<Rigidbody2D>().AddForce(dir.normalized * (-boom.GetComponent<Item>().fallSpeed * 2), ForceMode2D.Impulse);
    }
}
