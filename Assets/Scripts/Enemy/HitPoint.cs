using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 普通攻击造成20点伤害
            other.GetComponent<IDamageable>().GetHit(20);
        }

        if (other.CompareTag("Bomb"))
        {

        }
    }
}
