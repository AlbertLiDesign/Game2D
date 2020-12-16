﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // private变量不会显式表现出来
    // 获得人物刚体
    private Rigidbody2D rb;

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
    public GameObject jumpFX;
    public GameObject landFX;

    // 游戏一开始的时候执行的函数
    void Start()
    {
        // 获得为人物手动添加的rigidbody
        rb = GetComponent<Rigidbody2D>();
    }

    // 每一帧执行的函数，一般用来接收输入
    void Update()
    {
        CheckInput();
    }

    // 按秒执行，一秒执行50次，跟物理有关的执行方法放在FixedUpdate()
    public void FixedUpdate()
    {
        PhysicsCheck();
        Movement();
        Jump();
    }

    void CheckInput()
    {
        // 如果用户按下了跳跃键按键且处于地面时
        // Jump已经在unity中的Input Manager中定义好了，默认使用空格键
        if (Input.GetButtonDown("Jump") && isGround)
        {
            canJump = true;
        }
    }

    #region 键盘按键来移动
    void Movement()
    {
        // Horizontal已经在unity中的Input Manager中定义好了
        // 功能是通过左、右键或A、D键进行横向移动
        float horizontalInput = Input.GetAxis("Horizontal"); // -1 ~ 1 包括小数
        //float horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 ~ 1 不包括小数

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
}
