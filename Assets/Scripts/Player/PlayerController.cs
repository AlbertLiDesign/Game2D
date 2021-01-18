using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    // private变量不会显式表现出来
    // 获得人物刚体
    private Rigidbody2D rb;
    private Animator anim;
    private FixedJoystick joystick;

    // 横向移动
    public float speed; // 横向移动速度
    public float jumpForce; // 跳跃力
    public float gravityLevel; // 重力水平

    [Header("States Check")]
    public bool isGround; // 人物是否在地面上   
    public bool isJump; // 正在跳跃
    public bool canJump; // 是否按下跳跃

    [Header("Ground Check")]
    public Transform groundCheck; // 地面检测
    public float checkRadius; // 检测范围
    public LayerMask GroundLayer; // 检测图层

    [Header("Jump FX")]
    public GameObject jumpFX; // 跳跃特效
    public GameObject landFX; // 落地特效

    [Header("Attack Settings")]
    public GameObject bombPrefab;
    public float nextAttack = 0.0f; // 下次允许攻击的时间
    public float attackRate; // 技能CD

    [Header("Player State")]
    public float health; // 生命值
    public bool isDead; // 是否死亡

    private float nextJump = 0.0f; // 下次允许跳跃的时间
    private float JumpCD = 0.2f; // 跳跃CD


    // 游戏一开始的时候执行的函数
    void Start()
    {
        // 获得为人物手动添加的rigidbody
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        joystick = FindObjectOfType<FixedJoystick>();

        GameManager.instance.IsPlayer(this);

        health = GameManager.instance.LoadHealth();
        // 更新血量
        UIManager.instance.UpdateHealth(health);
    }

    // 每一帧执行的函数，一般用来接收输入
    void Update()
    {
        anim.SetBool("dead", isDead);
        if (isDead)
        {
            // 如果死亡，直接停止
            return;
        }
        CheckInput();
    }

    // 按秒执行，一秒执行50次，跟物理有关的执行方法放在FixedUpdate()
    public void FixedUpdate()
    {
        // 如果死亡，速度归0
        if (isDead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        PhysicsCheck();
        Movement();
        Jump();
    }

    void CheckInput()
    {
        // 如果用户按下了跳跃键按键且处于地面时
        // Jump已经在unity中的Input Manager中定义好了，默认使用空格键
        if (Input.GetKeyDown(KeyCode.K) && isGround && Time.time > nextJump)
        {
            nextJump = Time.time + JumpCD;
            canJump = true;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    #region 键盘按键来移动
    void Movement()
    {
        // Horizontal已经在unity中的Input Manager中定义好了
        // 功能是通过左、右键或A、D键进行横向移动
        float horizontalInput = Input.GetAxis("Horizontal"); // -1 ~ 1 包括小数
        //float horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 ~ 1 不包括小数

        // 操纵杆操作
        //float horizontalInput = joystick.Horizontal;

        // 定义小人左右移动，移动速度 = horizontalInput * speed，y方向（跳跃方向）移动时值不变
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        // 移动时，人的朝向也要对应到移动方向
        // 只需要用scale把 1 和 -1 进行颠倒即可
        if (horizontalInput != 0) // 没有原地不动时
        {
            var filp = horizontalInput > 0 ? 1 : -1;
            transform.localScale = new Vector3(filp, 1, 1);
        }
    }
    #endregion

    // 跳跃方法
    public void JumpButton()
    {
        if (isGround && Time.time > nextJump)
        {
            nextJump = Time.time + JumpCD;
            canJump = true;
        }
    }
    void Jump()
    {
        if (canJump)
        {
            isJump = true;

            // 跳跃特效激活
            jumpFX.SetActive(true);
            // 获取起跳点的position
            jumpFX.transform.position = transform.position + new Vector3(0.0f, -0.45f, 0.0f);

            // 跳跃移动速度
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            // 不能在空中跳跃
            canJump = false;

            // 更改重力，为了快速下降，重力增加
            rb.gravityScale = gravityLevel;
        }
    }

    public void Attack()
    {
        // 当前时间大于下次一可攻击的时间（技能有CD）
        if (Time.time > nextAttack)
        {
            // Instantiate将gameobject在场景中生成
            Instantiate(bombPrefab, transform.position, bombPrefab.transform.rotation);

            // 当前攻击时间+攻击间隔即为下次可攻击时间
            nextAttack = Time.time + attackRate;
        }
    }

    // 检测是否位于地面
    void PhysicsCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, GroundLayer);
        if (isGround)
        {
            // 改回重力
            rb.gravityScale = 1;
            isJump = false;
        }
    }


    // 下落特效 Animation event
    public void LandFX()
    {
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0.0f, -0.75f, 0.0f);
    }

    // 把检测范围可视化
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    public void GetHit(float damage)
    {
        // 获取动画图层第一层中的hit动画，受伤短暂无敌
        if (!anim.GetCurrentAnimatorStateInfo(1).IsName("Player_Hit"))
        {
            health -= damage;
            if (health < 1)
            {
                health = 0;
                isDead = true;
            }
            anim.SetTrigger("hit");

            // 更新血量
            UIManager.instance.UpdateHealth(health);
        }
    }
}
