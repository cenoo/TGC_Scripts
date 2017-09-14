using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/7/28 13:25:00
*功能：激光子弹的碰撞检测
*/
public class LazerBulletTrigger : MonoBehaviour
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
            bulletsPond.recycleLazerBullet(gameObject);
            other.SendMessage("HitByBullet", energy);
        }
        else if (other.tag == "Laser")
        {
            bulletsPond.recycleLazerBullet(gameObject);
        }
    }
}
