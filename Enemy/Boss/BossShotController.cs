using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/*
*作者：吴胜刚
*日期：2017/7/28 13:25:00
*功能：控制boss子弹和大招的发射
*/

class BossShotController : MonoBehaviour
{
    //boss发大招前会显示一个警告提示语，这个是提示语对应的prefab
    public GameObject warning;

    // 发射的子弹都包含在该对象下面，用于解决因为缩放比例造成的大小不一致问题
    private GameObject bulletParent;

    // 激光槽，视觉上激光子弹将从这个位置发射出去
    public Transform lazerSlot1;
    public Transform lazerSlot2;

    // 子弹池，发射时都从池子中获取子弹然后发射
    private Ponds bulletsPond;

    // 子弹槽，boss1中使用到
    public Transform bulletSlot;

    // 导弹槽，boss2的大招（导弹）将从这里发射出去    
    public Transform missileSlot;

    // 以下两个是boss2用到的，boss1没用，是气泡子弹发射的位置
    public Transform bulletSlot1;
    public Transform bulletSlot2;

    //当前的关卡数，不同关卡boss不同，部分子弹发射方式不同
    public int level;

    void Start()
    {
        bulletParent = GameObject.Find("EnemyController");
        bulletsPond = Ponds.getBulletsPond();
    }

    // boss1发射气泡子弹的逻辑
    private IEnumerator FireBubbleBulletsImpl(int count)
    {
        for (int i = 0; i <= count; i++)
        {
            bulletsPond.getRandomDirectionBullet().transform.position = bulletSlot.position;
            yield return new WaitForSeconds(0.05f);
        }
    }

    //boss2发射气泡子弹的逻辑
    private IEnumerator FireBubbleBulletsImpl2(int count)
    {
        for (int i = 0; i <= count / 2; i++)
        {
            bulletsPond.getRandomDirectionBullet().transform.position = bulletSlot1.position;
            bulletsPond.getRandomDirectionBullet().transform.position = bulletSlot2.position;
            yield return new WaitForSeconds(0.05f);
        }
    }

    // 发射气泡子弹
    public void FireBubbleBullets(int count)
    {
        if (level == 1)
            StartCoroutine(FireBubbleBulletsImpl(count));
        else StartCoroutine(FireBubbleBulletsImpl2(count));
    }

    // 发射导弹（大招）
    public void FireMissile()
    {
        if (level == 1)
            StartCoroutine(FireMissileImpl());
        else
            StartCoroutine(FireMissile2Impl());
    }

    // 发射激光子弹
    public void FireLazerBullets(int countOfWaves)
    {
        StartCoroutine(FireLazerBulletsImpl(countOfWaves));
    }

    IEnumerator FireLazerBulletsImpl(int countOfWaves)
    {
        for (int i = 0; i < countOfWaves; i++)
        {
            bulletsPond.getLazerBullet().transform.position = lazerSlot1.position;
            bulletsPond.getLazerBullet().transform.position = lazerSlot2.position;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator FireMissileImpl()
    {
        yield return showWarning();

        float yPos = 0.8f;
        for (int i = 0; i < 3; ++i)
        {
            GameObject bullet = bulletsPond.getBossMissile();
            bullet.transform.position = new Vector3(2.5f, yPos, -5);
            bullet = bulletsPond.getBossMissile();
            bullet.transform.position = new Vector3(2.5f, -yPos, -5);
            yield return new WaitForSeconds(0.75f);
            yPos -= 0.25f;
        }

        bulletsPond.getBossMissile().transform.position = new Vector3(2.5f, 0, -5);
    }

    private IEnumerator FireMissile2Impl()
    {
        yield return showWarning();
        int seqNum = 1;
        for (int i = 0; i < 3; ++i)
        {
           GameObject missile1 = bulletsPond.getBoss2Missile(seqNum);
            missile1.transform.position = missileSlot.position;
            seqNum = -seqNum;

            yield return new WaitForSeconds(0.25f);
            GameObject missile2 = bulletsPond.getBoss2Missile(seqNum);
            missile2.transform.position = missileSlot.position;
            seqNum = -seqNum;
            yield return new WaitForSeconds(0.25f);
        }
    }

    // 显示大招前面的警告提示语
    private IEnumerator showWarning()
    {
        GameObject warningObj = GenerateWarning(new Vector3(0, 0, -5)); //Instantiate(warning, new Vector3(0, 0, -5), Quaternion.identity);
        warningObj.transform.position = new Vector3(0, 0, -5);
        yield return new WaitForSeconds(0.5f);
        warningObj.transform.position = new Vector3(0, 0, 20);
        yield return new WaitForSeconds(0.1f);
        warningObj.transform.position = new Vector3(0, 0, -5);
        yield return new WaitForSeconds(0.5f);
        warningObj.transform.position = new Vector3(0, 0, 20);
        yield return new WaitForSeconds(0.1f);
        warningObj.transform.position = new Vector3(0, 0, -5);
        yield return new WaitForSeconds(0.5f);
        warningObj.transform.position = new Vector3(0, 0, 20);
        yield return new WaitForSeconds(0.1f);
        warningObj.transform.position = new Vector3(0, 0, -5);
        yield return new WaitForSeconds(1);
        warningObj.transform.position = new Vector3(0, 0, 20);
        Destroy(warningObj);
    }

    // 发射直角阵型的子弹
    public void FireRightAngleBullets(int countOfWaves)
    {
        StartCoroutine(FireRightAngleBulletsImpl(countOfWaves));
    }

    private IEnumerator FireRightAngleBulletsImpl(int countOfWaves)
    {
        for (int i = 0; i < countOfWaves; ++i)
        {
            for (int j = 0; j < 21; j++)
            {
                CircleBulletArg arg = new CircleBulletArg(j - 10, 0, false);
                arg.seqNum = j - 10;
                bulletsPond.getCircleBullet(arg).transform.position = bulletSlot.position;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    GameObject GenerateWarning(Vector3 position)
    {
        return GeneratorHelper.GeneratorGameObject(position, warning, gameObject.transform.parent.gameObject);
    }

    //发射一个弧线队形的弹幕，比如半圆形，圆形等，count * unitAngle等于弧线的圆形角
    //例如，要发射半圆形的弹幕，count * unitAngle = 180°，圆形弹幕则 count * unitAngle = 360°
    public void FireArcFormationBullet(int count, int unitAngle, int countOfWaves)
    {
        StartCoroutine(FireArcFormationBulletImpl(count, unitAngle, countOfWaves));
    }

    private IEnumerator FireArcFormationBulletImpl(int count, int unitAngle, int countOfWaves)
    {
        int mid = count / 2;
        for (int i = 0; i < countOfWaves; ++i)
        {
            for (int j = 0; j < count; j++)
            {
                CircleBulletArg arg = new CircleBulletArg(j - mid, unitAngle, true);
                GameObject bullet = bulletsPond.getCircleBullet(arg);
                bullet.transform.position = bulletSlot.position;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    // public void FireMultiwavesRightAngleBullets(int waves)
    // {
    //     StartCoroutine(FireMultiwavesRightAngleBulletsImpl(waves));
    // }

    // IEnumerator FireMultiwavesRightAngleBulletsImpl(int waves)
    // {
    //     for (int i = 0; i < waves; i++)
    //     {
    //         FireRightAngleBullets(1);
    //         yield return new WaitForSeconds(0.1f);
    //     }
    // }

}
