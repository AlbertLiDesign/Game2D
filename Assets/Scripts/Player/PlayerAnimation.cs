using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D rb;
    private PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        // 获取动画组件
        anim = GetComponent<Animator>();
        // 获取刚体组件
        rb = GetComponent<Rigidbody2D>();
        // 获取代码编写的PlayerController组件
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 把移动速度赋给speed参数
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        // 是否在下落
        anim.SetFloat("vY", rb.velocity.y);
        // 是否在跳跃状态
        anim.SetBool("jump", controller.isJump);
        // 是否在地面
        anim.SetBool("ground", controller.isGround);
    }
}
