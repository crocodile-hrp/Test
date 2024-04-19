using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("角色移动速度")]
    [SerializeField] public float moveSpeed;
    [Header("角色跳跃力度")]
    [SerializeField] float jumpForce;
    [Header("检测半径")]
    [SerializeField] float checkRadius;
    [Header("检测图层")]
    [SerializeField] LayerMask layerMask;
    [Header("护盾")]
    [SerializeField] GameObject shieldIcon;
    [Header("检测点")]
    [SerializeField] Transform checkPoint;
    [Header("动画控制器")]
    [SerializeField] Animator anim;
    [Header("物理材质")]
    [SerializeField] PhysicsMaterial2D[] phyMaterial;
    [Header("子弹预制体")]
    public GameObject bulletPrefabs;
    [Header("攻击Cd")]
    public float atkCd = 1;
    [Header("攻击力")]
    public int atk = 1;
    [Header("当前攻击Cd")]
    float currentAtkCd = 0;
    [Tooltip("移动方向")] int moveDir = 0;
    [Tooltip("刚体2d")] Rigidbody2D rb;
    [Tooltip("2d碰撞体")] Collider2D collider;
    [Tooltip("是否落地")] [SerializeField] bool isOnLand;
    [Tooltip("护盾时间")]public float shieldTime = 4;
    [Tooltip("护盾状态")] public bool isShield;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver || GameManager.Instance.bossIsDead)
            return;
        LandCheck();
        PlayerInput();
        PlayerAtk();
    }

    private void FixedUpdate()
    {
        ChangeAnimation();
        PlayerMove();
    }

    public void PlayerAtk()
    {
        if (GameManager.Instance.money >= 10)
        {
            if(currentAtkCd <= 0)
            {
                PoolManager.Instance.GetObj(bulletPrefabs).transform.position = transform.position+new Vector3(0,1);
                currentAtkCd = atkCd;
                GameManager.Instance.SetMoney(-10,false);
            }
            else
            {
                currentAtkCd -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// 护盾状态
    /// </summary>
    public void ShieldState()
    {
        StartCoroutine(IEShieldTimeDaley(shieldTime));
    }

    /// <summary>
    /// 护盾时效
    /// </summary>
    /// <returns></returns>
    IEnumerator IEShieldTimeDaley(float timer)
    {
        isShield = true;
        shieldIcon.SetActive(true);
        yield return new WaitForSeconds(timer);
        //TODO：后面可以改逻辑 时间快到 闪烁动画提示
        isShield = false;
        shieldIcon.SetActive(false);
    }

    bool LandCheck()
    {
        if (isOnLand)
            collider.sharedMaterial = phyMaterial[0];
        else
            collider.sharedMaterial = phyMaterial[1];
        return isOnLand = Physics2D.OverlapCircle(checkPoint.position, checkRadius, layerMask);
    }

    private void PlayerInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            moveDir = -1;
            transform.localScale = new Vector2(moveDir, 1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveDir = 1;
            transform.localScale = new Vector2(moveDir, 1);
        }
        else
            moveDir = 0;
        if (Input.GetKeyDown(KeyCode.Space)&& isOnLand)
            PlayerJump();
        if (JoyStick.Instance.movePos.x > 0.3f)
        {
            moveDir = 1;
            transform.localScale = new Vector2(-moveDir, 1);
        }else if(JoyStick.Instance.movePos.x < -0.3f)
        {
            moveDir = -1;
            transform.localScale = new Vector2(-moveDir, 1);
        }
        else
        {
            moveDir = 0;
        }
        if (JoyStick.Instance.movePos.y > 0.8f && isOnLand)
        {
            PlayerJump();
        }
    }

    private void PlayerJump()
    {
        //transform.position += new Vector3(rigidBody.velocity.x, jumpForce);
        isOnLand = false;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void PlayerMove()
    {
        rb.velocity = new Vector2(moveDir * moveSpeed, rb.velocity.y);
    }

    /// <summary>
    /// 动画切换 （随便写下 后面再改）
    /// </summary>
    private void ChangeAnimation()
    {
        anim.SetBool("jump",  !isOnLand);
        if (isOnLand)
            anim.SetFloat("speed", Mathf.Abs(moveDir));
            
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(checkPoint.position, checkRadius);
    }
#endif
}
