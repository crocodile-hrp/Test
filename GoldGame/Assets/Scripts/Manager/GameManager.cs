using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameManager : Singleton<GameManager>
{
    public Transform[] bolders;
    [Header("生命")]public int lift;
    [Header("金钱")] public int money;
    public static event Action GameOver;
    public GameObject bossPrefab;
    public int killBossCount;
    public int hitBossCount;
    public float gameTime;
    public bool isGameOver;
    public bool bossIsDead;

    protected override void Awake()
    {
        InitValue();
        bolders[0].position = Camera.main.ScreenToWorldPoint(new Vector3(-50, 0, 0));
        bolders[1].position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + 50, 0, 0));
        base.Awake();
    }

    private void Update()
    {
        if(lift <=0 && !isGameOver)
        {
            isGameOver = true;
            GameOver?.Invoke();
        }
        if (bossIsDead)
        {
            bossIsDead = false;
            Invoke("CreateBoss", 2f);
        }
        gameTime += Time.deltaTime;
    }

    public void StartGame()
    {
        InitValue();
    }

    public void SetMoney(int value)
    {
        money += value;
    }

    public void SetLift(int value)
    {
        lift += value;
    }

    void InitValue()
    {
        lift = 3;
        money = 0;
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
        killBossCount += 1;
    }

    public void CreateBoss()
    {
        bossIsDead = false;
        GameObject bossObj = PoolManager.Instance.GetObj(bossPrefab);
        bossObj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height + 50, 100));
    }

}
