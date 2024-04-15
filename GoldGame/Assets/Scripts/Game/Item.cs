using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    AddMoney,
    ReduceMoney,
    AddLift,
    Bomb,
}
public class Item : MonoBehaviour
{
    [Header("降落速度")]
    [SerializeField] float fallSpeed = -2;
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
        if (GameManager.Instance.gameTime <= 10)
            fallSpeed = Random.Range(-2, -1);
        else if(GameManager.Instance.gameTime <= 30)
            fallSpeed = Random.Range(-4, -1);
        else
            fallSpeed = Random.Range(-6, -1.2f);

    }

    private void OnDisable()
    {
        if (GameManager.Instance.gameTime <= 10)
            fallSpeed = Random.Range(-2, -1);
        else if (GameManager.Instance.gameTime <= 30)
            fallSpeed = Random.Range(-4, -1);
        else
            fallSpeed = Random.Range(-6, -1.2f);
        GameManager.GameOver -= ReleaseObj;
    }

    private void OnDestroy()
    {
        GameManager.GameOver -= ReleaseObj;
    }

    private void FixedUpdate()
    {
        ItemFalling();
    }

    /// <summary>
    /// 降落
    /// </summary>
    private void ItemFalling()
    {
        rb.velocity = new Vector2(0, fallSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Line"))
        {
            PoolManager.Instance.ReleaseObj(gameObject);
        }
        if (collision.CompareTag("Player"))
        {
            switch (ItemType) 
            {
                case ItemType.AddMoney:
                    GameManager.Instance.SetMoney(10);
                    break;
                case ItemType.ReduceMoney:
                    GameManager.Instance.SetMoney(Random.Range(-20,-5));
                    break;
                case ItemType.Bomb:
                    GameManager.Instance.SetLift(-1);
                    PoolManager.Instance.GetObj(prefab).transform.position = gameObject.transform.position;
                    break;
                case ItemType.AddLift:
                    GameManager.Instance.SetLift(1);
                    break;
            }

            PoolManager.Instance.ReleaseObj(gameObject);
        }
    }


    public void ReleaseObj()
    {
        PoolManager.Instance.ReleaseObj(gameObject);
    }
}
