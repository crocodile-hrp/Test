﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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
        //if (bossIsDead)
        //{
        //    bossIsDead = false;
        //    Invoke("CreateBoss", 2f);
        //}
        if(!bossIsDead)
            gameTime += Time.deltaTime;
    }

    public void StartGame()
    {
        InitValue();
    }

    public void SetMoney(int value)
    {
        money += value*moneyRatio;
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
        player.shieldTime = 3;
        player.atk = 1;
        player.atkCd = 2;
        gameTime = 0;
        hitBossCount = 0;
        killBossCount = 0;
        isGameOver = false;
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

    public void CreateBoss()
    {
        bossIsDead = false;
        GameObject bossObj = PoolManager.Instance.GetObj(bossPrefab);
        UIManager.Instance.bossHpHold.SetActive(true);
        bossObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height + 50, 100));
    }

}