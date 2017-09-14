using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
*作者：吴胜刚
*日期：2017/8/5 26:20:13
*功能：控制每个关卡的 boss的行为方式，何时射击，何时放大招，何时移动到某一点等；
*IBossProxy接口定义了可以在boss身上进行的操作。
*BossProxy类实现了IBossProxy接口，但是只是简单的将方法调用转发给内部的BossProxyImpl类的对象，这样设计是为了将对boss的行为控制的流程和具体的某一个行为的实现分离开
*/

public class BossController : MonoBehaviour
{
    private Rigidbody2D body;

    private BossProxy boss;

    public int level = 1;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boss = new BossProxy();
        boss.setImplementation(GetComponent<BossProxyImpl>());
        if (level == 2)
            Boss2Behavior();
        else Boss1Behavior();
    }

    //boss的行为，此为满血到半血时间段的行为；
    private void Boss1Behavior()
    {
        boss.FireMissiles();
        boss.MoveTo(new Vector3(1.5f, 0, -5), 0.5f);
        boss.StayForSeconds(1.5f);
        boss.LoopStart();
        boss.MoveTo(new Vector3(1f, 0, -5), 0.5f);
        boss.FireLazerBullets(2);
        boss.StayForSeconds(3);
        boss.FireLazerBullets(2);
        boss.StayForSeconds(0.5f);
        boss.FireBubbleBullets(10);
        boss.DiveForPlayer();
        boss.FireMissiles();
        boss.MoveTo(new Vector3(1.5f, 0.6f, -5), 0.5f);
        boss.StayForSeconds(1);
        boss.FireLazerBullets(3);
        boss.StayForSeconds(2);
        boss.FireLazerBullets(3);
        boss.MoveTo(new Vector3(1.5f, -0.5f, -5), 0.5f);
        boss.FireArcFormationBullets(18, 20, 1);
        boss.MoveTo(new Vector3(1.5f, 0, -5), 0.5f);
        boss.FireMissiles();
        boss.StayForSeconds(2);
        boss.FireBubbleBullets(10);
        boss.MoveTo(new Vector3(1f, 0.5f, -5), 0.5f);
        boss.StayForSeconds(1);
        boss.FireLazerBullets(3);
        boss.DiveForPlayer();
        boss.FireLazerBullets(1);
        boss.MoveTo(new Vector3(1f, -0.5f, -5), 0.5f);
        boss.StayForSeconds(2);
        boss.FireBubbleBullets(15);
        boss.MoveTo(new Vector3(1.5f, 0, -5), 0.5f);
        boss.FireMissiles();
        boss.LoopEnd();
    }

    private void Boss2Behavior()
    {
        boss.LoopStart();
        boss.MoveTo(new Vector3(1.5f, 0, -5), 0.5f);
        boss.FireMissiles();
        boss.StayForSeconds(1);
        boss.MoveTo(new Vector3(1f, 0, -5), 0.5f);
        boss.FireLazerBullets(2);
        boss.StayForSeconds(3);
        boss.FireLazerBullets(2);
        boss.StayForSeconds(0.5f);
        boss.FireBubbleBullets(10);
        boss.DiveForPlayer();
        boss.FireMissiles();
        boss.MoveTo(new Vector3(1.5f, 0.6f, -5), 0.5f);
        boss.StayForSeconds(1);
        boss.FireLazerBullets(3);
        boss.StayForSeconds(2);
        boss.FireLazerBullets(3);
        boss.MoveTo(new Vector3(1.5f, -0.5f, -5), 0.5f);
        boss.FireArcFormationBullets(18, 20, 1);
        boss.MoveTo(new Vector3(1.5f, 0, -5), 0.5f);
        boss.FireMissiles();
        boss.StayForSeconds(2);
        boss.FireBubbleBullets(10);
        boss.MoveTo(new Vector3(1f, 0.5f, -5), 0.5f);
        boss.StayForSeconds(1);
        boss.FireLazerBullets(3);
        boss.DiveForPlayer();
        boss.FireLazerBullets(1);
        boss.MoveTo(new Vector3(1f, -0.5f, -5), 0.5f);
        boss.StayForSeconds(2);
        boss.FireBubbleBullets(15);
        boss.MoveTo(new Vector3(1.5f, 0, -5), 0.5f);
        boss.FireMissiles();
        boss.LoopEnd();
    }

    //boss的半血到四分之一血时候的行为，发射的子弹更多更密集；
    private void BossBehaviorOfHalfBlood()
    {
        boss.LoopStart();
        boss.MoveTo(new Vector3(1.5f, 0, -5), 1f);
        boss.FireRightFormationBullets(1);
        boss.StayForSeconds(0.5f);
        boss.MoveTo(new Vector3(1.5f, 0.3f, -5), 1.5f);
        boss.MoveTo(new Vector3(1.5f, -0.3f, -5), 1.5f);
        boss.FireBubbleBullets(15);
        boss.FireLazerBullets(4);
        boss.MoveTo(new Vector3(1.5f, 0.3f, -5), 1.5f);
        boss.MoveTo(new Vector3(1.5f, -0.3f, -5), 1.5f);
        boss.FireArcFormationBullets(18, 20, 3);
        boss.FireMissiles();
        boss.DiveForPlayer();
        boss.StayForSeconds(0.5f);
        boss.FireLazerBullets(4);
        boss.FireBubbleBullets(20);
        boss.LoopEnd();
    }

    private void BossBehaviorOfQuaterBlood()
    {
        boss.LoopStart();
        boss.FireBubbleBullets(30);
        boss.MoveTo(new Vector3(0, 0, -5), 2f);
        boss.FireArcFormationBullets(18, 20, 4);
        boss.MoveTo(new Vector3(1.5f, 0, -5), 1.5f);
        boss.FireLazerBullets(5);
        boss.StayForSeconds(1);
        boss.FireLazerBullets(5);
        boss.DiveForPlayer();
        boss.FireMissiles();
        boss.MoveTo(new Vector3(1.5f, 0.6f, -5), 2f);
        boss.StayForSeconds(1);
        boss.FireArcFormationBullets(30, 12, 6);
        boss.MoveTo(new Vector3(1.5f, -0.6f, -5), 2f);
        boss.StayForSeconds(1);
        boss.FireRightFormationBullets(3);
        boss.MoveTo(new Vector3(0, 0, -5), 2f);
        boss.FireMissiles();
        boss.StayForSeconds(1);
        boss.FireArcFormationBullets(24, 15, 4);
        boss.MoveTo(new Vector3(-1.5f, 0, -5), 2);
        boss.FireLazerBullets(5);
        boss.StayForSeconds(1);
        boss.FireArcFormationBullets(30, 12, 5);
        boss.MoveTo(new Vector3(1.5f, 0, -5), 2f);
        boss.LoopEnd();
    }

    //当boss半血时将会收到该方法的回调，在这里可以给boss设置新的行为流程，
    //在设置新的行为的时候需要调用boss.RemoveALlInstruction()清楚之前设置的行为
    public void OnBossHalfBlood(float blood)
    {
        Debug.Log("boss half blood. boss blood = " + blood);
        boss.RemoveAllInstructions(true);
        BossBehaviorOfHalfBlood();
    }

    //方法解释同OnBossHalfBlood();
    public void OnBossQuarterBlood(float blood)
    {
        Debug.Log("boss quater blood. boss blood = " + blood);
        boss.RemoveAllInstructions(true);
        BossBehaviorOfQuaterBlood();
    }

    private class BossProxy : IBossProxy
    {
        private IBossProxy bossProxyImpl;

        public void setImplementation(IBossProxy bossProxyImpl)
        {
            this.bossProxyImpl = bossProxyImpl;
        }

        public void DiveForPlayer()
        {
            bossProxyImpl.DiveForPlayer();
        }

        public void FireArcFormationBullets(int count, int unitAngle, int countOfWaves)
        {
            bossProxyImpl.FireArcFormationBullets(count, unitAngle, countOfWaves);
        }

        public void FireBubbleBullets(int count)
        {
            bossProxyImpl.FireBubbleBullets(count);
        }

        public void FireLazerBullets(int count)
        {
            bossProxyImpl.FireLazerBullets(count);
        }

        public void FireMissiles()
        {
            bossProxyImpl.FireMissiles();
        }

        public void FireRightFormationBullets(int countOfWaves)
        {
            bossProxyImpl.FireRightFormationBullets(countOfWaves);
        }

        public void LoopEnd()
        {
            bossProxyImpl.LoopEnd();
        }

        public void LoopStart()
        {
            bossProxyImpl.LoopStart();
        }

        public void MoveTo(Vector3 targetPoint, float speed)
        {
            bossProxyImpl.MoveTo(targetPoint, speed);
        }

        public void RemoveAllInstructions(bool immediate)
        {
            bossProxyImpl.RemoveAllInstructions(immediate);
        }

        public void StayForSeconds(float seconds)
        {
            bossProxyImpl.StayForSeconds(seconds);
        }
    }
}
