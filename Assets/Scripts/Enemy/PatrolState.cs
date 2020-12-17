using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyBaseState
{
    // 进入状态
    public override void EnterState(Enemy enemy)
    {
        enemy.animState = 0;
        enemy.SwitchPoint();
    }

    // 执行巡逻
    public override void OnUpdate(Enemy enemy)
    {
        if (!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            // 开始移动
            enemy.animState = 1;
            enemy.MoveToTarget();
        }

        // 如果走到了目标点，就切换目标点
        if (Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x) < 0.01f)
        {
            // 切换回巡逻状态
            enemy.TransitionToState(enemy.patrolState);
        }

        // 如果看到敌人目标
        if (enemy.attackList.Count > 0)
        {
            // 切换为攻击状态
            enemy.TransitionToState(enemy.attackState);
        }

    }
}
