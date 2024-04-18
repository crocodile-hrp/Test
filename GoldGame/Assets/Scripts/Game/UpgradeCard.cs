using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CardType
{
    Life,
    Atk,
    Move,
    AtkCd,
    Money,
    ShieldTime,
}
public class UpgradeCard : MonoBehaviour
{
    [Header("�����ı�")]public Text Info;
    [Header("������ť")] public GameObject noBtn;
    public CardType cardType;
    [Header("ˢ�»����ı�")]public Text costText;
    public int flushCount = 1;

    private void OnEnable()
    {
        flushCount = 1;
        InitCard();
    }

    void InitCard()
    {
        float random = Random.Range(0, 1f);
        if (random < 0.4f)
        {
            noBtn.SetActive(false);
        }
        else
        {
            noBtn.SetActive(true);
        }
        int randomType = Random.Range(0, 6);
        switch (randomType)
        {
            case 0:
                cardType = CardType.Life;
                Info.text = "��������+1(�зⶥ)";
                break;
            case 1:
                cardType = CardType.Atk;
                Info.text = "����������1(�зⶥ)";
                break;
            case 2:
                cardType = CardType.AtkCd;
                Info.text = "����������ٰٷ�֮ʮ(�зⶥ)";
                break;
            case 3:
                cardType = CardType.Move;
                Info.text = "�ƶ��ٶ����Ӱٷ�֮ʮ(�зⶥ)";
                break;
            case 4:
                cardType = CardType.Money;
                Info.text = "��һ�ȡ����+1";
                break;
            case 5:
                cardType = CardType.ShieldTime;
                Info.text = "����ʱ��+1";
                break;
        }
        costText.text = (10 * flushCount) + "���ˢ�´���";
    }

    public void OnYes()
    {
        switch (cardType)
        {
            case CardType.Life:
                if(GameManager.Instance.maxlife< 10)
                    GameManager.Instance.maxlife += 1;
                break;
            case CardType.Atk:
                if (GameManager.Instance.player.atk < 10)
                    GameManager.Instance.player.atk += 1;
                break;
            case CardType.AtkCd:
                if (GameManager.Instance.player.atkCd > 0.2)
                    GameManager.Instance.player.atkCd -= GameManager.Instance.player.atkCd*0.1f;
                break;
            case CardType.Move:
                if (GameManager.Instance.player.moveSpeed < 7)
                    GameManager.Instance.player.moveSpeed += GameManager.Instance.player.moveSpeed * 0.1f;
                break;
            case CardType.Money:
                GameManager.Instance.moneyRatio += 1;
                    break;
            case CardType.ShieldTime:
                if (GameManager.Instance.player.shieldTime < 12)
                    GameManager.Instance.player.shieldTime += 1;
                break;
        }
        GameManager.Instance.life = GameManager.Instance.maxlife;
        GameManager.Instance.bossIsDead = false;
        GameManager.Instance.CreateBoss();
        gameObject.SetActive(false);
    }

    public void OnFlush()
    {
        GameManager.Instance.SetMoney(-10*flushCount,false);
        flushCount++;
        InitCard();
    }

    public void OnNo()
    {
        GameManager.Instance.life = GameManager.Instance.maxlife;
        GameManager.Instance.bossIsDead = false;
        GameManager.Instance.CreateBoss();
        gameObject.SetActive(false);
    }

}
