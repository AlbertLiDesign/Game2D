using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuy : Enemy, IDamageable
{
    // 拿起炸弹的位置
    public Transform pickupPoint;
    // 扔出炸弹的力度
    public float power;
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
    public void PickUpBomb()
    {
        // 如果目标是炸弹且手里没有炸弹就可以捡起来了
        if (targetPoint.CompareTag("Bomb") && !hasBomb)
        {
            targetPoint.gameObject.transform.position = pickupPoint.position;

            // 将炸弹变成Big Guy的子集
            targetPoint.SetParent(pickupPoint);

            // 取消炸弹的重力
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            hasBomb = true;
        }
    }

    // Animation Event
    public void ThrowAway()
    {
        if (hasBomb)
        {
            // 改回炸弹的重力
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            // 返回到Big Guy的层级
            targetPoint.SetParent(transform.parent.parent);

            // 找到挂载PlayController的物体
            if (FindObjectOfType<PlayerController>().transform.position.x - transform.position.x < 0)
            {
                // 向左侧扔炸弹
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1) * power, ForceMode2D.Impulse);
            }
            else
            {
                // 向左侧扔炸弹
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1) * power, ForceMode2D.Impulse);
            }

            hasBomb = false;
        }
    }
}
