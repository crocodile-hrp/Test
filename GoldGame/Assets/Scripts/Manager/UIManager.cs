using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text hpText;
    public Text moneyText;
    public Text scoreText;
    public GameObject bossHpHold;
    public GameObject endPlane;

    private void Start()
    {
        InitUI();
    }

    private void OnEnable()
    {
        GameManager.GameOver += ShowEndUI;
    }

    private void OnDisable()
    {
        GameManager.GameOver -= ShowEndUI;
    }

    private void OnDestroy()
    {
        GameManager.GameOver -= ShowEndUI;
    }
    private void InitUI()
    {
        hpText.text = "✖" + GameManager.Instance.lift;
        moneyText.text = "✖" + GameManager.Instance.money;
        bossHpHold.SetActive(true);
    }

    private void Update()
    {
        hpText.text = "✖" + GameManager.Instance.lift;
        moneyText.text = "✖" + GameManager.Instance.money;
    }

    public void ShowEndUI()
    {
        bossHpHold.SetActive(false);
        endPlane.SetActive(true);
        scoreText.text = "击中Boss："+GameManager.Instance.hitBossCount+" 次，"+"击败Boss：" + GameManager.Instance.killBossCount +" 个";
    }

    public void OnContinueBtn()
    {
        InitUI();
        endPlane.SetActive(false);
        GameManager.Instance.StartGame();
    }

}
