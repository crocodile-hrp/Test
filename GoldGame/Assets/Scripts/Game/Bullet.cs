using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("基础飞行速度")]
    [SerializeField] float baseSpeed = 2;
    [Tooltip("当前飞行速度")]
    [SerializeField] float currentFlySpeed = 2;
    [Header("物品类型")]
    [SerializeField] ItemType ItemType = ItemType.AddMoney;
    [Tooltip("刚体2d")] Rigidbody2D rb;
    [Tooltip("2d碰撞体")] Collider2D collider;
    [Header("子弹精灵图")] public Sprite[] bulletSprites;
    [Header("子弹Icon")] public SpriteRenderer bulletIcons;
    [Header("生成后续物品")]
    [SerializeField] GameObject prefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        //bulletIcons.sprite = bulletSprites[GameManager.Instance.player.atk - 1];
        currentFlySpeed = baseSpeed * GameManager.Instance.bulletFlySpeed;
        GameManager.GameOver += ReleaseObj;
        GameManager.BossDead += ReleaseObj;
    }

    private void OnDisable()
    {
        GameManager.GameOver -= ReleaseObj;
        GameManager.BossDead -= ReleaseObj;
    }

    private void OnDestroy()
    {
        GameManager.GameOver -= ReleaseObj;
        GameManager.BossDead -= ReleaseObj;
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.GetChild(0).Rotate(Vector3.back * 120f * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        BulletFlying();
    }

    /// <summary>
    /// 降落
    /// </summary>
    private void BulletFlying()
    {
        rb.velocity = new Vector2(0, currentFlySpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            collision.GetComponent<Enemy>().OnHit(GameManager.Instance.player.atk);
            PoolManager.Instance.ReleaseObj(gameObject);
        }
        if (collision.CompareTag("Line"))
        {
            PoolManager.Instance.ReleaseObj(gameObject);
        }
    }


    public void ReleaseObj()
    {
        PoolManager.Instance.ReleaseObj(gameObject);
    }
}
