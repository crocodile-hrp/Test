using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Text hpText;
    public Text moneyText;
    public Text scoreText;
    public GameObject bossHpHold;
    public GameObject endPlane;
    public GameObject UpgradeCard;

    protected void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitUI();
    }

    private void OnEnable()
    {
        GameManager.GameOver += ShowEndUI;
        GameManager.BossDead += ShowUpgradeCard;
    }

    private void OnDisable()
    {
        GameManager.GameOver -= ShowEndUI;
        GameManager.BossDead -= ShowUpgradeCard;
    }

    private void OnDestroy()
    {
        GameManager.GameOver -= ShowEndUI;
        GameManager.BossDead -= ShowUpgradeCard;
    }
    private void InitUI()
    {
        hpText.text = "✖" + GameManager.Instance.life;
        moneyText.text = "✖" + GameManager.Instance.money;
        bossHpHold.SetActive(true);
    }

    private void Update()
    {
        hpText.text = "✖" + GameManager.Instance.life;
        moneyText.text = "✖" + GameManager.Instance.money;
    }

    public void ShowEndUI()
    {
        bossHpHold.SetActive(false);
        endPlane.SetActive(true);
        scoreText.text = "生存时间："+GameManager.Instance.gameTime+" 秒，"+"击败Boss：" + GameManager.Instance.killBossCount +" 个";
    }

    public void ShowUpgradeCard()
    {
        UpgradeCard.SetActive(true);
    }

    public void OnContinueBtn()
    {
        InitUI();
        endPlane.SetActive(false);
        GameManager.Instance.StartGame();
    }

}
