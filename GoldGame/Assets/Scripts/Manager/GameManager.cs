using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class GameManager : Singleton<GameManager>
{
    public Transform[] bolders;
    [Header("当前生命")]public int life;
    [Header("最大生命")] public int maxlife = 3;
    [Header("金钱")] public int money;
    [Header("玩家类")] public Player player;
    [Header("金钱获得倍率")] public int moneyRatio = 1;
    [Tooltip("游戏结束事件")]public static event Action GameOver;
    [Tooltip("击败Boss事件")] public static event Action BossDead;
    public GameObject bossPrefab;
    public int killBossCount;
    public int hitBossCount;
    public float gameTime;
    public bool isGameOver;
    public bool bossIsDead;
    [Tooltip("是否为第一只生成的boss")]public bool isFirstBoss = true;//是否为第一只生成的boss
    public float firstInitBossTime = 20f;

    protected override void Awake()
    {
        player = FindObjectOfType<Player>();
        InitValue();
        bolders[0].position = Camera.main.ScreenToWorldPoint(new Vector3(-50, 0, 0));
        bolders[1].position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + 50, 0, 0));
        base.Awake();
    }

    private void Update()
    {
        if(life <=0 && !isGameOver)
        {
            isGameOver = true;
            GameOver?.Invoke();
        }
        if(!bossIsDead)
            gameTime += Time.deltaTime;
    }

    public void StartGame()
    {
        InitValue();
    }

    /// <summary>
    /// 设置金币
    /// </summary>
    /// <param name="value">值</param>
    /// <param name="haveRatio">是否有倍率影响（默认true）</param>
    public void SetMoney(int value,bool haveRatio = true)
    {
        if (haveRatio)
            money += value * moneyRatio;
        else
            money += value;
    }

    public void SetLift(int value)
    {
        life += value;
    }

    void InitValue()
    {
        maxlife = 3;
        life = maxlife;
        money = 0;
        moneyRatio = 1;
        player.shieldTime = 4;
        player.atk = 1;
        player.atkCd = 2;
        gameTime = 0;
        hitBossCount = 0;
        killBossCount = 0;
        isGameOver = false;
        //Invoke("CreateBoss", 5f);
        isFirstBoss = true;
        CreateBoss();
    }

    public void AddHitCount()
    {
        hitBossCount += 1;
    }

    public void AddKillBoss()
    {
        bossIsDead = true;
        BossDead?.Invoke();
        killBossCount += 1;
    }

    /// <summary>
    /// 生成boss
    /// </summary>
    public void CreateBoss()
    {
        if (isFirstBoss)
        {
            Invoke("BossPrefabsCreate", firstInitBossTime);
            //BossPrefabsCreate();
        }
        else
        {
            BossPrefabsCreate();
        }
    }

    /// <summary>
    /// 基础生成boss逻辑
    /// </summary>
    void BossPrefabsCreate()
    {
        bossIsDead = false;
        isFirstBoss = false;
        UIManager.Instance.bossHpHold.SetActive(true);
        GameObject bossObj = PoolManager.Instance.GetObj(bossPrefab);
        //boss出场小位移
        bossObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height - 150, 100));
        //bossObj.transform.DOMove(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height + 50, 100)), 2f);
    }
}
