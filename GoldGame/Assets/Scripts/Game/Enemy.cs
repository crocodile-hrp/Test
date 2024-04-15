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
    }

    void InitBoss()
    {
        currentHp = maxHp;
        bossHp.fillAmount = currentHp / maxHp;
        hitMask.color = new Color(hitMask.color.r, hitMask.color.g, hitMask.color.b, 0);
    }


    private void OnDisable()
    {
        GameManager.GameOver -= InitBoss;
        InitBoss();
    }
    private void OnDestroy()
    {
        GameManager.GameOver -= InitBoss;
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

}
