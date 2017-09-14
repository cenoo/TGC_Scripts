using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
*作者：吴胜刚
*日期：2017/7/31 15:59:27
*功能：控制boss发射的导弹（大招）的移动，从boss身后以直线运动射出
*/
public class BossMissileMover : MonoBehaviour
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
        // 超出屏幕自动销毁
        if (Mathf.Abs(transform.position.x) > 3 || Mathf.Abs(transform.position.y) > 1)
        {
            bulletsPond.recycleBossMissile(gameObject);
        }
    }
}