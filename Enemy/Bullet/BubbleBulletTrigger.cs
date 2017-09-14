using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/7/28 13:25:00
*功能：控制气泡子弹的碰撞检测，击中玩家飞机或者其大招激光的时候销毁自身（回收到子弹池中）
*/

public class BubbleBulletTrigger : MonoBehaviour
{
    public float energy;
    private Ponds bulletsPond;

    void Start()
    {
        bulletsPond = Ponds.getBulletsPond();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            bulletsPond.recycleRandomDirectionBullet(gameObject);
            other.SendMessage("HitByBullet", energy);
        }
        else if (other.tag == "Laser")
        {
            bulletsPond.recycleRandomDirectionBullet(gameObject);
        }
    }

}
