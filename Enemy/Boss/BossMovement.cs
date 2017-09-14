using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/8/1 16:03:44
*功能：控制boss的移动，在BossProxyImpl脚本中调用该脚本中的相应方法
*/

public class BossMovement : MonoBehaviour
{

    public float speed;

    public float fastSpeed;
    
    private Rigidbody2D body;

    private GameObject player;

    private BossProxyImpl bossProxyImpl;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        bossProxyImpl = GetComponent<BossProxyImpl>();

    }

    // 控制boss运动到某一个位置
    public void MoveTo(Vector3 targetPoint, float speed)
    {
        StartCoroutine(MoveToImpl(targetPoint, speed, true));

    }

    private IEnumerator MoveToImpl(Vector3 targetPoint, float speed, bool changeState)
    {
        Vector3 movement = targetPoint - transform.position;
        float distance = movement.magnitude;
        float time = distance / speed;
        body.velocity = movement.normalized * speed;
        yield return new WaitForSeconds(time);
        transform.position = targetPoint;
        body.velocity = Vector3.zero;
        // 回调BossProxyImpl里面的OnBossMoveCompleted()方法，通知其移动已经结束，可以改变其相应的状态
        //MoveTo、DiveForPlayer和StayingForSeconds等方法的执行都会回调这个，因为它们内部使用的实现是相同的
        bossProxyImpl.OnBossMoveCompleted(changeState);
    }

    // 控制boss向玩家进行一次俯冲
    public void DiveForPlayer()
    {
        StartCoroutine(DiveForPlayerImpl());
    }

    private IEnumerator DiveForPlayerImpl()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        Vector3 oldPoint = transform.position;
        yield return MoveToImpl(player.transform.position, fastSpeed, false);
        yield return stayingForSecondsImpl(0.5f, false);
        yield return MoveToImpl(oldPoint, fastSpeed, true);
    }

    // 让boss待在现在的地点一段时间
    public void stayingForSeconds(float seconds)
    {
        StartCoroutine(stayingForSecondsImpl(seconds, true));
    }


    private IEnumerator stayingForSecondsImpl(float seconds, bool changeState)
    {
        yield return new WaitForSeconds(seconds);
        bossProxyImpl.OnBossMoveCompleted(changeState);
    }

}
