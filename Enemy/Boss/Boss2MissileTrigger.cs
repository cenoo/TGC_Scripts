using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/8/6 14:20:13
*功能：控制Boss2的导弹（大招）的触发器
*/

public class Boss2MissileTrigger : MonoBehaviour {

	public float energy;
    private Ponds bulletsPond;

    void Awake()
    {
        bulletsPond = Ponds.getBulletsPond();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            bulletsPond.recycleBoss2Missile(gameObject);
            other.SendMessage("HitByBullet", energy);
        }
        else if (other.tag == "Laser")
        {
            bulletsPond.recycleBoss2Missile(gameObject);
        }
    }
}
