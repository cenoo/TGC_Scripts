using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
*作者：吴胜刚
*日期：2017/7/31 10:10:11
*功能：boss发射的随机方向的子弹的控制逻辑，先以一个高速发射许多子弹,然后控制子弹减速、加速，x和y方向的速度都是随机值，产生一种“喷射”的效果
*加减速使用的是多次试验得出的一元一次方程。
*该子弹一开始命名为RandomDirectionBullet,后来修改了贴图之后子弹样子很像一个气泡，所有又改名叫BubbleBullet,但是部分游戏资源和脚本中的函数名称没有修改
*/

public class RandomDirectionBulletMover : MonoBehaviour
{
    public float baseSpeed;
    public float startSpeed;
    private float startTime;

    private Rigidbody2D body;
    private float kX;
    private float kY;
    private float originYVelocity;

    private Ponds bulletsPond;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        bulletsPond = Ponds.getBulletsPond();
    }

    void Init()
    {
        originYVelocity = Random.Range(-0.2f, 0.2f);
        float targetXVelocity = baseSpeed - Random.Range(-0.4f, 0.56f);
        float targetYVelocity = Random.Range(-0.8f, 0.8f);
        kY = targetYVelocity / 4f;
        kX = targetXVelocity / 4f;
        body = GetComponent<Rigidbody2D>();
        body.velocity = new Vector3(-startSpeed, originYVelocity, 0);
        startTime = Time.time;
    }

    void Update()
    {
        float deltaTime = Time.time - startTime;
        if (deltaTime > 0.2f && deltaTime < 1f)
        {
            body.velocity = new Vector3(-(-1.5f * deltaTime + 1.7f), body.velocity.y, 0);
        }

        if (deltaTime > 2 && deltaTime < 6)
        {
            body.velocity = new Vector3(-(kX * (deltaTime - 2f) + 0.2f), kY * (deltaTime - 2f) + originYVelocity, 0);
        }
    }

    // 当子弹超出屏幕时，销毁子弹
    void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x) > 1.8 || Mathf.Abs(transform.position.y) > 1)
        {
            bulletsPond.recycleRandomDirectionBullet(gameObject);
        }
    }
}