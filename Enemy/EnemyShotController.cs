using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
*作者：吴胜刚
*日期：2017/7/31 15:20:31
*功能：控制敌机发射子弹
*/

public class EnemyShotController : MonoBehaviour
{
    // 敌机的武器槽，敌机的子弹从这里发射出来
    public Transform weaponSlot;
    private BulletType bulletType;

    private GameObject bulletsPondObject;
    private Ponds bulletsPond;

    private float nextFire;
    public float fireRate = 1f;

    void Start()
    {
        bulletsPondObject = GameObject.Find("EnemyBulletPond");
        bulletsPond = bulletsPondObject.GetComponent<Ponds>();
        nextFire = Time.time + fireRate;
    }

    void Update()
    {
        if (Time.time > nextFire)
        {
            Fire();
            nextFire = Time.time + fireRate;
        }
    }

    void Fire()
    {
        if (bulletType == BulletType.CIRCLE_BULLET)
        {
            CircleBulletArg arg = new CircleBulletArg(-1, 5, true);
            bulletsPond.getCircleBullet(arg).transform.position = weaponSlot.position;
            arg.seqNum = 1;
            bulletsPond.getCircleBullet(arg).transform.position = weaponSlot.position;
        }
        else
        {
            bulletsPond.getLazerBullet().transform.position = weaponSlot.position;
        }
    }

    public void setBulletType(BulletType type)
    {
        bulletType = type;   
    }
}

public enum BulletType
{
    CIRCLE_BULLET, LAZER_BULLET, BUBBLE_BULLET
}