using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHp = 100;
    public float currentHp;
    public SpriteRenderer hitMask;
    public Image bossHp;

    /// <summary>
    /// Boss初始移动距离
    /// </summary>
    public float initRange = 15f;
    /// <summary>
    /// Boss到达移速最大所需击杀点数
    /// </summary>
    public float toMaxSpeedKillcount = 3;

    private void Awake()
    {
        bossHp = GameObject.Find("BossHp").GetComponent<Image>();
    }
    private void Start()
    {
        InitBoss();
    }

    private void OnEnable()
    {
        GameManager.GameOver += InitBoss;
        GameManager.BossDead += InitBoss;
        GameManager.GameOver += ReleaseObj;
        BossInitMove();
    }

    private void FixedUpdate()
    {
        BossSkillRelease();
    }

    void InitBoss()
    {
        if(GameManager.Instance.killBossCount<=3)
            maxHp = Random.Range(15, 20);
        else if(GameManager.Instance.killBossCount <= 10)
            maxHp = Random.Range(25, 50);
        else
            maxHp = Random.Range(100, 150);
        currentHp = maxHp;
        bossHp.fillAmount = currentHp / maxHp;
        hitMask.color = new Color(hitMask.color.r, hitMask.color.g, hitMask.color.b, 0);
    }


    private void OnDisable()
    {
        GameManager.GameOver -= InitBoss;
        GameManager.BossDead -= InitBoss;
        GameManager.GameOver -= ReleaseObj;
    }
    private void OnDestroy()
    {
        GameManager.GameOver -= InitBoss;
        GameManager.BossDead -= InitBoss;
        GameManager.GameOver -= ReleaseObj;
    }

    public void OnHit(float damage)
    {
        currentHp -= damage;
        GameManager.Instance.AddHitCount();
        bossHp.fillAmount = currentHp / maxHp;
        StartCoroutine(BossHitAnim());
        if(currentHp <= 0)
        {
            GameManager.Instance.AddKillBoss();
            UIManager.Instance.bossHpHold.SetActive(false);
            PoolManager.Instance.ReleaseObj(gameObject);
        }
    }

    IEnumerator BossHitAnim()
    {
        hitMask.color = new Color(hitMask.color.r, hitMask.color.g, hitMask.color.b, 1);
        yield return new WaitForSeconds(0.5f);
        hitMask.color = new Color(hitMask.color.r, hitMask.color.g, hitMask.color.b, 0);
        yield break;
    }
    public void ReleaseObj()
    {
        PoolManager.Instance.ReleaseObj(gameObject);
    }

    /// <summary>
    /// Boss移动初始化
    /// </summary>
    void BossInitMove()
    {
        transform.DOMove(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height + 50, 100)), 2f).OnComplete(() => BossRandomMove());
    }

    /// <summary>
    /// Boss跟随死亡次数随机移动
    /// </summary>
    void BossRandomMove()
    {
        float range;
        float num;

        range = Mathf.Lerp(initRange, Screen.width / 2, Mathf.Clamp01(GameManager.Instance.killBossCount / toMaxSpeedKillcount));
        num = Random.Range(Screen.width / 2 - range, Screen.width / 2 + range);
        transform.DOMove(Camera.main.ScreenToWorldPoint(new Vector3(num, Screen.height + 50, 100)), 2f).OnComplete(() => BossRandomMove());
    }

    /// <summary>
    /// Boss技能释放
    /// </summary>
    void BossSkillRelease()
    {
        if (currentHp / maxHp <= 0.25f)
        {
            //释放随机技能?
            int skillType = Random.Range(0, 2);
            switch (skillType)
            {
                case 0:
                    Debug.Log("释放激光？");
                    break;
                case 1:
                    Debug.Log("释放夹子？");
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Boss动作动画播放
    /// </summary>
    void BossAnim()
    {

    }
}
