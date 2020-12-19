using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captain : Enemy, IDamageable
{
    SpriteRenderer sprite;
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

    public override void Init()
    {
        base.Init();

        sprite = GetComponent<SpriteRenderer>();
    }

    public override void Update()
    {
        base.Update();

        if (animState == 0)
        {
            sprite.flipX = false;
        }
    }


    public override void SkillAction()
    {
        base.SkillAction();

        if (anim.GetCurrentAnimatorStateInfo(1).IsName("Skill"))
        {
            // 转身
            sprite.flipX = true;
            // 看到炸弹往反方向跑
            if (transform.position.x > targetPoint.position.x)
            {
                transform.position = Vector2.MoveTowards
                    (transform.position, transform.position + Vector3.right, speed * 3 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards
                     (transform.position, transform.position + Vector3.left, speed * 3 * Time.deltaTime);
            }
        }
        else
        {
            sprite.flipX = false;
        }
    }
}