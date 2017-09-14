using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
*作者：吴胜刚
*日期：2017/7/31 15:17:50
*功能：控制的敌机或者其子弹的碰撞检测
*/

public class EnemyTriggerHandler : MonoBehaviour
{
    private Ponds bulletsPond;

    void Start()
    {
        bulletsPond = Ponds.getBulletsPond();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("onTriggerEnter");
            Debug.Log(gameObject);
        }
    }
}