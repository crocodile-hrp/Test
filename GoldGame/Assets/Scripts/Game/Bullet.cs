using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("�����ٶ�")]
    [SerializeField] float fallSpeed = 2;
    [Header("��Ʒ����")]
    [SerializeField] ItemType ItemType = ItemType.AddMoney;
    [Tooltip("����2d")] Rigidbody2D rb;
    [Tooltip("2d��ײ��")] Collider2D collider;
    [Header("�ӵ�����ͼ")] public Sprite[] bulletSprites;
    [Header("�ӵ�Icon")] public SpriteRenderer bulletIcons;
    [Header("���ɺ�����Ʒ")]
    [SerializeField] GameObject prefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        bulletIcons.sprite = bulletSprites[GameManager.Instance.player.atk - 1];
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

    private void FixedUpdate()
    {
        BulletFlying();
    }

    /// <summary>
    /// ����
    /// </summary>
    private void BulletFlying()
    {
        rb.velocity = new Vector2(0, fallSpeed);
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
