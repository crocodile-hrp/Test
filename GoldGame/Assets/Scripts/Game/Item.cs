using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    AddMoney,
    ReduceMoney,
    AddLift,
    Bomb,
    BombBeat,
    Shield,
    Atk,
}
public class Item : MonoBehaviour
{
    [Header("降落速度")]
    [SerializeField] public float fallSpeed = -2;
    [Header("物品类型")]
    [SerializeField] ItemType ItemType = ItemType.AddMoney;
    [Tooltip("刚体2d")] Rigidbody2D rb;
    [Tooltip("2d碰撞体")] Collider2D collider;
    [Header("生成后续物品")]
    [SerializeField] GameObject prefab;

    [Header("---物品掉落速度详细属性---")]
    //物品掉落速度详细属性
    [Header("到最大速度所需时间")] public float minToMaxTime = 30;
    [Header("最大速度")] public float maxSpeed = 6f;
    [Header("最小速度")] public float minSpeed = 1.2f;
    [Header("随机范围")] public float randomRange = 1f;
    [Header("是否为debuff道具")] public bool isDebuff;

    bool isfirstHit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        GameManager.GameOver += ReleaseObj;
        GameManager.BossDead += ReleaseObj;
        //if (GameManager.Instance.gameTime <= 10)
        //    fallSpeed = Random.Range(-2, -1);
        //else if (GameManager.Instance.gameTime <= 30)
        //    fallSpeed = Random.Range(-4, -1);
        //else
        //    fallSpeed = Random.Range(-6, -1.2f);

        //新
        if(GameManager.Instance.killBossCount >= 2)//两只boss后发力
        {
            if (GameManager.Instance.gameTime <= minToMaxTime)
            {
                fallSpeed = Mathf.Lerp(-minSpeed, -maxSpeed, Mathf.Clamp01(GameManager.Instance.gameTime / minToMaxTime)) + Random.Range(-randomRange, randomRange);
            }
            else
            {
                fallSpeed = -maxSpeed + Random.Range(-randomRange, randomRange);
            }
        }
        else
        {
            if (GameManager.Instance.gameTime <= minToMaxTime)
            {
                fallSpeed = Mathf.Lerp(-minSpeed, -maxSpeed*0.5f, Mathf.Clamp01(GameManager.Instance.gameTime / minToMaxTime)) + Random.Range(-randomRange, randomRange);
            }
            else
            {
                fallSpeed = -maxSpeed* 0.5f + Random.Range(-randomRange, randomRange);
            }
        }

   
    }

    private void OnDisable()
    {
        //if (GameManager.Instance.gameTime <= 10)
        //    fallSpeed = Random.Range(-2, -1);
        //else if (GameManager.Instance.gameTime <= 30)
        //    fallSpeed = Random.Range(-4, -1);
        //else
        //    fallSpeed = Random.Range(-6, -1.2f);

        //反击炸弹相关回到初始状态
        isfirstHit = false;
        beatBack = false;

        if (GameManager.Instance == null) return;
        //新
        if (GameManager.Instance.killBossCount >= 2)//两只boss后发力
        {
            if (GameManager.Instance.gameTime <= minToMaxTime)
            {
                fallSpeed = Mathf.Lerp(-minSpeed, -maxSpeed, Mathf.Clamp01(GameManager.Instance.gameTime / minToMaxTime)) + Random.Range(-randomRange, randomRange);
            }
            else
            {
                fallSpeed = -maxSpeed + Random.Range(-randomRange, randomRange);
            }
        }
        else
        {
            if (GameManager.Instance.gameTime <= minToMaxTime)
            {
                fallSpeed = Mathf.Lerp(-minSpeed, -maxSpeed * 0.5f, Mathf.Clamp01(GameManager.Instance.gameTime / minToMaxTime)) + Random.Range(-randomRange, randomRange);
            }
            else
            {
                fallSpeed = -maxSpeed * 0.5f + Random.Range(-randomRange, randomRange);
            }
        }

        GameManager.GameOver -= ReleaseObj;
        GameManager.BossDead -= ReleaseObj;
    }

    private void OnDestroy()
    {
        GameManager.GameOver -= ReleaseObj;
        GameManager.BossDead -= ReleaseObj;
    }

    bool beatBack = false;

    private void FixedUpdate()
    {
        if (!beatBack)
        {
            ItemFalling();
        }

        if(ItemType == ItemType.BombBeat)
        {
            if (gameObject.activeInHierarchy)
            {
                //小旋转
                transform.Rotate(Vector3.forward * 180 * Time.deltaTime);
                if (beatBack)
                {
                    ItemManager.Instance.BeatBack(gameObject, GameManager.Instance.bossObj);
                }
            }
        }
    }

    /// <summary>
    /// 降落
    /// </summary>
    private void ItemFalling()
    {
        //判断是否释放时停技能
        if (!ItemManager.Instance.isPause)
        {
            rb.velocity = new Vector2(0, fallSpeed);
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
            //新 若是debuff物品 直接清除
            if (isDebuff)
            {
                PoolManager.Instance.ReleaseObj(gameObject);
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Line"))
        {
            PoolManager.Instance.ReleaseObj(gameObject);
        }
        //反击判断
        if (ItemType == ItemType.BombBeat)
        {
            if (collision.CompareTag("Boss"))
            {
                //炸弹初次生成似乎在boss体内 加此判断避免boss自杀
                if (!isfirstHit)
                {
                    isfirstHit = true;
                }
                else
                {
                    collision.GetComponent<Enemy>().OnHit(GameManager.Instance.player.atk);
                    PoolManager.Instance.GetObj(prefab).transform.position = gameObject.transform.position;
                    PoolManager.Instance.ReleaseObj(gameObject);
                }
            }
        }
        if (collision.CompareTag("Player"))
        {
            switch (ItemType) 
            {
                case ItemType.AddMoney:
                    GameManager.Instance.SetMoney(10);
                    break;
                case ItemType.ReduceMoney:
                    if (!GameManager.Instance.player.isShield)//非护盾下
                    {
                        if(GameManager.Instance.gameTime <= 30)
                            GameManager.Instance.SetMoney(Random.Range(-10, -5));
                        else
                            GameManager.Instance.SetMoney(Random.Range(-20, -5));
                    }
                    break;
                case ItemType.Bomb:
                    if (!GameManager.Instance.player.isShield)//非护盾下
                    {
                        GameManager.Instance.SetLift(-1);
                        PoolManager.Instance.GetObj(prefab).transform.position = gameObject.transform.position;
                    }
                    break;
                case ItemType.BombBeat:
                    if (!GameManager.Instance.player.isShield)//非护盾下
                    {
                        //若在跳跃状态 则炸弹返回炸boss
                        if (!GameManager.Instance.player.isOnLand)
                        {
                            beatBack = true;
                        }
                        else
                        {
                            GameManager.Instance.SetLift(-1);
                            PoolManager.Instance.GetObj(prefab).transform.position = gameObject.transform.position;
                        }
                    }
                    break;
                case ItemType.AddLift:
                    GameManager.Instance.SetLift(1);
                    break;
                case ItemType.Shield:
                    collision.GetComponent<Player>().ShieldState();
                    break;
                case ItemType.Atk:
                    collision.GetComponent<Player>().AtkState();
                    break;
            }
            if(ItemType!=ItemType.BombBeat)
                PoolManager.Instance.ReleaseObj(gameObject);
        }
    }


    public void ReleaseObj()
    {
        PoolManager.Instance.ReleaseObj(gameObject);
    }
}
