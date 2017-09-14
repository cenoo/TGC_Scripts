using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/7/31 15:59:27
*功能：boss1的 飞弹的移动控制
*/
public class BossMissileTrigger : MonoBehaviour
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
            bulletsPond.recycleBossMissile(gameObject);
            other.SendMessage("HitByBullet", energy);
        }
        else if (other.tag == "Laser")
        {
            bulletsPond.recycleBossMissile(gameObject);
        }
    }
}
