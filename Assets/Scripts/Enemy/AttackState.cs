using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        // 将攻击目标先以最先发现的目标
        enemy.animState = 2; // 进入攻击状态
        enemy.targetPoint = enemy.attackList[0];
    }

    public override void OnUpdate(Enemy enemy)
    {
        // 如果没有人了，就切换回巡逻状态
        if (enemy.attackList.Count == 0)
        {
            enemy.TransitionToState(enemy.patrolState);
        }

        // 如果有多个目标
        if (enemy.attackList.Count>1)
        {
            for (int i = 0; i < enemy.attackList.Count; i++)
            {
                if (Mathf.Abs(enemy.transform.position.x - enemy.attackList[i].position.x) 
                    < Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x))
                {
                    enemy.targetPoint = enemy.attackList[i];
                }
            }
        }
        if (enemy.attackList.Count == 1)
        {
            enemy.targetPoint = enemy.attackList[0];
        }

        // 如果目标点是玩家，那么普通攻击
        if (enemy.targetPoint.CompareTag("Player"))
        {
            enemy.AttackAction();
        }

        // 如果目标是炸弹，那么使用技能
        if (enemy.targetPoint.CompareTag("Bomb"))
        {
            enemy.SkillAction();
        }
        enemy.MoveToTarget();
    }
}
