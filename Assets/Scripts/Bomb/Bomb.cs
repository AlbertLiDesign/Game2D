using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    private Rigidbody2D rid;

    public float startTime;
    public float waitTime;
    public float bombForce;

    [Header("Check")]
    public float radius;
    public LayerMask targetLayer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rid = GetComponent<Rigidbody2D>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Bomb_off"))
        {
            if (Time.time > startTime + waitTime)
            {
                // 计时器结束，引爆炸弹
                anim.Play("Explotion");
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }


    // Animation Event
    public void Explotion()
    {
        // 碰撞检测不检测自身
        coll.enabled = false;

        // 爆炸时检测环境
        // 获得爆炸中心点一定范围内所有发生碰撞的物体
        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        // 不检测自身但是有可能掉落出屏幕，所以这时要改重力
        rid.gravityScale = 0;

        foreach (var item in aroundObjects)
        {
            // 获得被炸物体相对炸弹的方向
            Vector3 pos = transform.position - item.transform.position;
            // 为每一个被炸物体添加一个冲击力
            item.GetComponent<Rigidbody2D>().AddForce((-pos + Vector3.up) * bombForce, ForceMode2D.Impulse);

            // 爆炸可以引燃另一个被关闭的炸弹
            if (item.CompareTag("Bomb") && item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Bomb_off"))
            {
                item.GetComponent<Bomb>().TurnOn();
            }
        }
    }

    // 销毁炸弹
    public void DestroyBomb()
    {
        Destroy(gameObject);
    }

    public void TurnOff()
    {
        anim.Play("Bomb_off");
        gameObject.layer = LayerMask.NameToLayer("NPC");
    }

    public void TurnOn()
    {
        startTime = Time.time;
        anim.Play("Bomb_on");
        gameObject.layer = LayerMask.NameToLayer("Bomb");
    }
}
