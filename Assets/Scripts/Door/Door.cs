using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    BoxCollider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();

        GameManager.instance.IsExitDoor(this);
        coll.enabled = false;
    }

    // 供Game Manager调用的开门方法
    public void OpenDoor()
    {
        anim.Play("Opening");
        coll.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 去到下一个房间
            GameManager.instance.NextLevel();
        }
    }
}
