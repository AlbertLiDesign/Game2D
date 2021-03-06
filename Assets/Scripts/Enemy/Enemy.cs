﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    EnemyBaseState currentState;
    public bool isBoss;
    public Animator anim;
    // 动画状态
    public int animState;

    private GameObject alarmSign;

    [Header("Attack Setting")]
    public float attackRate; // 普通攻击间隔
    public float attackRange; // 普通攻击距离
    public float skillRate; // 技能攻击间隔
    public float skillRange; // 技能攻击距离

    private float nextAttack = 0.0f;
    private float nextSkill = 0.0f;

    [Header("Base State")]
    public float health; // 生命值
    public bool isDead; // 是否死亡
    public bool hasBomb; // 已经拿起炸弹的状态

    [Header("Movement")]
    public float speed;
    // 巡逻两点
    public Transform pointA, pointB;
    // 目标点
    public Transform targetPoint;

    // 范围内的多个目标列表
    public List<Transform> attackList = new List<Transform>();

    // 巡逻状态
    public PatrolState patrolState = new PatrolState();
    // 攻击状态
    public AttackState attackState = new AttackState();

    // 初始化方法
    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        // 获取警告标识
        alarmSign = transform.GetChild(0).gameObject;
    }

    // Awake优先于Start调用
    public void Awake()
    {
        Init();

    }
    // Start is called before the first frame update
    void Start()
    {
        // 注册敌人
        GameManager.instance.IsEnemy(this);
        // 开始巡逻状态
        TransitionToState(patrolState);

        if (isBoss)
        {
            UIManager.instance.SetBossHealth(health);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        anim.SetBool("dead", isDead);

        if (isDead)
        {
            // 敌人死后从敌人列表中移除
            GameManager.instance.EnemyDead(this);
            return;
        }

        // 执行巡逻状态
        currentState.OnUpdate(this);
        // 每一帧都检查当前的参数
        anim.SetInteger("state", animState);

        if (isBoss)
        {
            UIManager.instance.UpdateBossHealth(health);
        }
    }

    // 变换当前状态
    public void TransitionToState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    // 敌人移动功能
    public void MoveToTarget()
    {
        // 向目标移动
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        
        FilpDirection();
    }

    // 普通攻击，加入virtual关键字使其子类可以修改该方法
    public void AttackAction()
    {
        // 如果玩家位于攻击范围内
        if (Vector2.Distance(transform.position, targetPoint.position)<attackRange)
        {
            // CD时间满足
            if (Time.time > nextAttack)
            {
                // 播放攻击动画
                anim.SetTrigger("attack");
                nextAttack = Time.time + attackRate;
            }
        }
    }

    // 技能攻击，加入virtual关键字使其子类可以修改该方法
    public virtual void SkillAction()
    {
        // 如果炸弹位于攻击范围内
        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            // CD时间满足
            if (Time.time > nextSkill)
            {
                // 播放技能动画
                anim.SetTrigger("skill");
                nextSkill = Time.time + skillRange;
            }
        }
    }

    // 转身
    public void FilpDirection()
    {
        if (transform.position.x < targetPoint.position.x)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f));
        }
    }

    // 切换目标点
    public void SwitchPoint()
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;
        }
        else
        {
            targetPoint = pointB;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        // 如果第一次看到目标，那么设为攻击目标
        if (!attackList.Contains(collision.transform) && !hasBomb && !isDead
            && !GameManager.instance.gameOver)
        {
            attackList.Add(collision.transform);
        }
        
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        attackList.Remove(collision.transform);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead && !GameManager.instance.gameOver)
        {
            // 刚进入检测范围时，发出警告
            StartCoroutine(onAlarm());
        }

    }

    // 协程处理
    IEnumerator onAlarm()
    {
        alarmSign.SetActive(true);

        yield return new WaitForSeconds(
            alarmSign.GetComponent<Animator>()
            .GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);
    }
}
