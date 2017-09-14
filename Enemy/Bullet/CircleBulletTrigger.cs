using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/7/28 13:25:00
*功能：控制圆形子弹的碰撞检测
*/

public class CircleBulletTrigger : MonoBehaviour
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
            bulletsPond.recycleCircleBullet(gameObject);
            other.SendMessage("HitByBullet", energy);
        }
        else if (other.tag == "Laser")
        {
            bulletsPond.recycleCircleBullet(gameObject);
        }
    }
}
