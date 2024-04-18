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
    [Header("描述文本")]public Text Info;
    [Header("放弃按钮")] public GameObject noBtn;
    public CardType cardType;
    [Header("刷新花费文本")]public Text costText;
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
                Info.text = "生命上限+1(有封顶)";
                break;
            case 1:
                cardType = CardType.Atk;
                Info.text = "攻击力上升1(有封顶)";
                break;
            case 2:
                cardType = CardType.AtkCd;
                Info.text = "攻击间隔减少百分之十(有封顶)";
                break;
            case 3:
                cardType = CardType.Move;
                Info.text = "移动速度增加百分之十(有封顶)";
                break;
            case 4:
                cardType = CardType.Money;
                Info.text = "金币获取倍率+1";
                break;
            case 5:
                cardType = CardType.ShieldTime;
                Info.text = "护盾时间+1";
                break;
        }
        costText.text = (10 * flushCount) + "金币刷新词条";
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
