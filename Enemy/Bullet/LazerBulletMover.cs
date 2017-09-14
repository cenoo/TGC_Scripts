using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/*
*作者：吴胜刚
*日期：2017/7/28 13:38:09
*功能：激光子弹的运动方式
*/

class LazerBulletMover : MonoBehaviour
{
    public float speed;

    private Ponds bulletsPond;
    void Start()
    {
        bulletsPond = Ponds.getBulletsPond();
    }

    void Init()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(-1, 0, 0) * speed;
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x) > 1.8 || Mathf.Abs(transform.position.y) > 1)
        {
            bulletsPond.recycleLazerBullet(gameObject);
        }
    }
}
