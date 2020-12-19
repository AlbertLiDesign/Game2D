using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Enemy, IDamageable
{
    public void GetHit(float damage)
    {
        health -= damage;
        if (health < 1)
        {
            health = 0;
            isDead = true;
        }
        anim.SetTrigger("hit");
    }

    // Animation Event
    public void SetOff()
    {
        // 吹灭炸弹技能
        targetPoint.GetComponent<Bomb>().TurnOff();
    }
}
