using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("降落速度")]
    [SerializeField] float fallSpeed = 2;
    [Header("物品类型")]
    [SerializeField] ItemType ItemType = ItemType.AddMoney;
    [Tooltip("刚体2d")] Rigidbody2D rb;
    [Tooltip("2d碰撞体")] Collider2D collider;
    [Header("生成后续物品")]
    [SerializeField] GameObject prefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        GameManager.GameOver += ReleaseObj;

    }

    private void OnDisable()
    {
        GameManager.GameOver -= ReleaseObj;
    }

    private void OnDestroy()
    {
        GameManager.GameOver -= ReleaseObj;
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
        rb.velocity = new Vector2(0, fallSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            collision.GetComponent<Enemy>().OnHit(1);
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
