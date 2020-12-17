using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Enemy
{
    // Animation Event
    public void SetOff()
    {
        targetPoint.GetComponent<Bomb>().TurnOff();
    }
}
