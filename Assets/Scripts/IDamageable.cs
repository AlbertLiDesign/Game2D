using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 受伤接口
// 接口方法跟抽象类相似，在这里需要写函数声明
// 然后在子类里实现方法
public interface IDamageable
{
    // 受伤函数
    void GetHit(float damage);

}
