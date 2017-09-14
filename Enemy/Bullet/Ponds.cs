using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/8/2 11:31:27
*功能：敌机的子弹（包括boss的导弹）的子弹池，
*/

public class Ponds : MonoBehaviour
{

    //保存一个对池子的引用，方便需要使用池子的脚本获取池子，具体见getBulletsPond()方法
    private static Ponds singletonPonds;

    private const int DEFAULT_CAPACITY = 16;

    //圆形子弹的对象池，弹幕队形成圆弧型的那种子弹
    private List<GameObject> CircleBulletPond = new List<GameObject>(DEFAULT_CAPACITY);

    //随机方向子弹的对象池，看起来类似气泡那种子弹
    private List<GameObject> RandomDirectionBulletPond = new List<GameObject>(DEFAULT_CAPACITY);

    //激光子弹的对象池
    private List<GameObject> LazerBulletPond = new List<GameObject>(DEFAULT_CAPACITY);

    //boss1的导弹的对象池，出现warning字样后发射的子弹
    private List<GameObject> BossMissilePond = new List<GameObject>(6);

    private List<GameObject> Boss2MissilePond = new List<GameObject>(6);

    //prefab的引用，用于Instantiate()方法中clone出新的对应的子弹类型的gameobject
    public GameObject circleBullet;
    public GameObject lazerBullet;

    public GameObject randomDirectionBullet;

    public GameObject bossMissile;

    public GameObject boss2Missile;

    //因为缩放的关系，需要给每个子弹的gameobject指定一个transform.parent，这样才能正常显示大小
    private GameObject bulletParent;

    //start方法里初始化
    void Start()
    {
        bulletParent = GameObject.Find("EnemyController");
        // initPond();
    }

    //getXXBullet()方法表示从池子中获取对象来使用，不同的对象有类似的方法，子弹的类型不同，可能需要不同的参数
    //获取子弹的时候会调用子弹对应的移动控制脚本中的 Init()方法，进行初始化设置
    public GameObject getCircleBullet(CircleBulletArg arg)
    {
        GameObject bullet = getBulletFrom(CircleBulletPond, circleBullet);
        bullet.SendMessage("Init", arg);
        return bullet;
    }

    public GameObject getRandomDirectionBullet()
    {
        GameObject bullet = getBulletFrom(RandomDirectionBulletPond, randomDirectionBullet);
        bullet.SendMessage("Init");
        return bullet;
    }

    public GameObject getBossMissile()
    {
        GameObject bullet = getBulletFrom(BossMissilePond, bossMissile);
        bullet.SendMessage("Init");
        return bullet;
    }

    public GameObject getLazerBullet()
    {
        GameObject bullet = getBulletFrom(LazerBulletPond, lazerBullet);
        bullet.SendMessage("Init");
        return bullet;
    }

    public GameObject getBoss2Missile(int seqNum)
    {
        GameObject bullet = getBulletFrom(Boss2MissilePond, boss2Missile);
        bullet.SendMessage("Init", seqNum);
        Debug.Log("get missile 2");
        return bullet;
    }

    //recycleXXBullet(GameObject bullet)方法用于回收使用完的对象，将其放回对象池
    public void recycleCircleBullet(GameObject bullet)
    {
        recycleBullet(CircleBulletPond, bullet);
    }

    public void recycleRandomDirectionBullet(GameObject bullet)
    {
        recycleBullet(RandomDirectionBulletPond, bullet);
    }

    public void recycleBossMissile(GameObject bullet)
    {
        recycleBullet(BossMissilePond, bullet);
    }

    public void recycleLazerBullet(GameObject bullet)
    {
        recycleBullet(LazerBulletPond, bullet);
    }

    public void recycleBoss2Missile(GameObject bullet)
    {
        recycleBullet(Boss2MissilePond, bullet);
    }

    //具体的回收对象的逻辑
    private void recycleBullet(List<GameObject> pond, GameObject bullet)
    {
        bullet.SetActive(false);
        pond.Add(bullet);
    }

    //具体的获取池子里对象的逻辑
    private GameObject getBulletFrom(List<GameObject> bulletsPond, GameObject bulletPrefab)
    {
        //池子里没有对象时，添加新的对象到池子里
        if (bulletsPond.Count == 0)
            addNewBullets(bulletsPond, bulletPrefab);
        //从list中取最后一个对象返回，并删除
        GameObject bullet = bulletsPond[bulletsPond.Count - 1];
        bullet.SetActive(true);
        bulletsPond.RemoveAt(bulletsPond.Count - 1);
        return bullet;
    }

    //添加新的对象到池子里，根据池子的容量，将池子直接用新对象填满
    //因为c#的list没有实现扩容方法，所以直接扩大容量不容易，只是简单粗暴的填满list
    //很可能在一次addNewBullets()方法后，整个场景里的某一种子弹对象会数倍于池子的大小，但是当执行一次回收操作后，池子容量会自然扩大
    private void addNewBullets(List<GameObject> where, GameObject bulletPrefab)
    {
        int capacity = where.Capacity;
        int countOfNewBullets;
        //子弹池的容量较小时，按照翻倍的方式快速增加池子里的对象的数量，因为此时很可能数量不足；
        //当子弹池容量较大时，则每次增加一颗子弹，防止无用对象太多占用资源
        if (capacity <= 20)
            countOfNewBullets = capacity;
        else
        {
            countOfNewBullets = 1;
        }
        for (int i = 0; i < countOfNewBullets; i++)
        {
            GameObject bullet = GenerateBulletHelper.GenerateBullets(bulletPrefab, new Vector3(0, 0, 0), bulletParent);
            bullet.SetActive(false);
            where.Add(bullet);
        }
    }

    //很多脚本里需要有个对象池的引用，所以统一通过该方法获取对池子的引用
    //因为池子只用一个（或者说需要大家使用同一个池子），如果每次都调用GameObject.Find()方法有性能损失，所以使用了一种类似单例的方式实现
    public static Ponds getBulletsPond()
    {
        if (singletonPonds == null)
            singletonPonds = GameObject.Find("EnemyBulletPond").GetComponent<Ponds>();

        return singletonPonds;
    }
}
