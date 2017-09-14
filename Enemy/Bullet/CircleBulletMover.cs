using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/*
*作者：吴胜刚
*日期：2017/7/28 17:50:38
*功能：控制boss发射的圆形子弹的移动，有几种不同的移动阵型，圆弧型，直角型
*/
class CircleBulletMover : MonoBehaviour
{
    private int seqNumber;

    private Ponds bulletsPond;

    public float speed;
    
    //子弹差值，控制直角阵型中的x和y方向的速度
    public float speedDelta;

    void Awake()
    {
        bulletsPond = Ponds.getBulletsPond();
    }

    void Init(CircleBulletArg arg)
    {
        seqNumber = arg.seqNum;
        if (arg.isArc)
        {
            ArcMove(arg.unitAngle);
        }
        else
        {
            RightAngleMove();
        }
    }

    void RightAngleMove()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector3(-1 * (speed - speedDelta * Mathf.Abs(seqNumber)), speedDelta * seqNumber, 0);
    }

    //unitAngle表示两个子弹间的夹角，设置为17度时可以射出圆形的子弹（17*21 == 360）
    void ArcMove(int unitAngle)
    {
        int angle = seqNumber * unitAngle;
        float rad = angle * Mathf.Deg2Rad;
        float xVelocity = speed * Mathf.Cos(rad) * -1;
        float yVelocity = speed * Mathf.Sin(rad);
        GetComponent<Rigidbody2D>().velocity = new Vector3(xVelocity, yVelocity, 0);
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x) > 1.8 || Mathf.Abs(transform.position.y) > 1)
        {
            bulletsPond.recycleCircleBullet(gameObject);
        }
    }

    void CircleMove()
    {
    }

}

public class CircleBulletArg
{
    public int seqNum;

    public int unitAngle;

    public bool isArc;

    public CircleBulletArg(int seqNum, int unitAngle, bool isArc)
    {
        this.seqNum = seqNum;
        this.unitAngle = unitAngle;
        this.isArc = isArc;
    }
}