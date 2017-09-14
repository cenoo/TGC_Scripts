using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/8/7 17:03:50
*功能：控制boss2的导弹（大招）的移动
*先向着左上方或者左下方运动，停留0.5秒后，向着玩家运动
*/

public class BossMissile2Movement : MonoBehaviour
{
    private Rigidbody2D body;

    public float speed;

    private float startTime;
    private Ponds bulletsPond;
    private GameObject player;
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        bulletsPond = Ponds.getBulletsPond();
    }

    IEnumerator Move(int seqNum)
    {
        float x = -Random.Range(0.5f, 1f);
        float y;
        if (seqNum > 0)
            y = Random.Range(1f, 1.5f);
        else y = Random.Range(-1.5f, -1f);
        body.velocity = new Vector3(x, y, 0).normalized * speed;
        yield return new WaitForSeconds(0.5f);
        body.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        body.velocity = (player.transform.position - transform.position).normalized * speed * 2;
    }

    void FixedUpdate()
    {
        // 让导弹自身有一定旋转
        float pastTime = Time.time - startTime;
        float angle = pastTime / 4f * 360;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 控制飞出屏幕的子弹销毁
        if (Mathf.Abs(transform.position.x) > 3 || Mathf.Abs(transform.position.y) > 1)
        {
            bulletsPond.recycleBossMissile(gameObject);
        }
    }

    //敌机的子弹（也包括boss的飞弹）都会有这个方法，在子弹池中获取子弹时都会调用该方法来设置初始的参数
    public void Init(int seqNUm)
    {
        startTime = Time.time;
        StartCoroutine(Move(seqNUm));
    }
}
