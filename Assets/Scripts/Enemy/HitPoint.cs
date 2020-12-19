using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    public bool bombAvilable; // 可以使用炸弹
    public float strength; // 击飞炸弹的力的强度
    private int dir; // 方向
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.position.x > other.transform.position.x)
        {
            // 炸弹在左侧
            dir = -1;
        }
        else
        {
            //炸弹在右侧
            dir = 1;
        }

        if (other.CompareTag("Player"))
        {
            // 普通攻击造成20点伤害
            other.GetComponent<IDamageable>().GetHit(20);
        }

        if (other.CompareTag("Bomb") && bombAvilable)
        {
            // 将炸弹击飞
            other.gameObject.GetComponent<Rigidbody2D>().
                AddForce(new Vector2(dir, 1.0f) * strength, ForceMode2D.Impulse);
        }
    }
}
