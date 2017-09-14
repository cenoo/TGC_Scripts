using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/8/7 10:10:46
*功能：boss的行为控制的中心，实现了一个类似消息队列的指令队列，改脚本不断从队列里取出指令（也就是各种对boss的行为控制），然后根据指令调用对应脚本里的方法
*实现了IBossProxy接口，BossController中对BossProxy的方法调用都会调用到这里面对应的方法。
*这些方法将构造对应的Instruction对象，设置正确的参数，然后放入instructionQueue中
*在FixedUpdate()方法中会不断取出指令来执行。
*/

public class BossProxyImpl : MonoBehaviour, IBossProxy
{
    //实现boss射击的脚本
    private BossShotController shotController;
    //boss移动的脚本
    private BossMovement moveController;

    //指令队列，控制的指令都放在这里面
    private Queue<Instruction> instructionQueue = new Queue<Instruction>();

    //只要boss没有被消灭，它就应该一直在行动，所有IBossProxy中有了LoopStart和LoopEND方法，两个方法调用之间的所有指令将会被循环执行
    //为了实现指令的循环执行，使用一个historyQueue，记录需要循环的指令
    private Queue<Instruction> historyQueue = new Queue<Instruction>();

    //记录boss的状态，不同的状态可以执行的指令不同；
    private BossState bossState;

    //调用了loopStart()方法，则loopStarted为true,后续执行过的指令将会放入historyQueue中，知道LoopEnd()调用
    private bool loopStarted = false;

    void Start()
    {
        shotController = GetComponent<BossShotController>();
        moveController = GetComponent<BossMovement>();
    }

    void FixedUpdate()
    {
        Instruction ins = null;
        //指令队列为空时，直接返回
        //有几条指令（）需要特殊处理,或者需要根据Instruction.type决定instruction是否马上执行，所以先使用peek()取出而不出队
        if (instructionQueue.Count > 0)
            ins = instructionQueue.Peek();
        else
            return;
        if (ins == null)
            return;
        //如果是REMOVE_ALL, LOOP_START,LOOP_END三种队列控制指令，则直接执行,然后返回。
        switch (ins.type)
        {
            case InstructionType.REMOVE_ALL:
                instructionQueue.Clear();
                historyQueue.Clear();
                return;
            case InstructionType.LOOP_START:
                if (!loopStarted)
                {
                    loopStarted = true;
                    instructionQueue.Dequeue();
                    historyQueue.Enqueue(ins);
                }
                else
                {
                    instructionQueue.Dequeue();
                }
                return;
            case InstructionType.LOOP_END:
                instructionQueue.Dequeue();
                historyQueue.Enqueue(ins);
                Queue<Instruction> temp = instructionQueue;
                instructionQueue = historyQueue;
                historyQueue = temp;
                historyQueue.Clear();
                loopStarted = false;
                return;
        }
        //设置boss进行移动或者待在某一位置一段时间的指令必须等待boss的前一个移动或者等待指令结束
        //射击指令可以在移动或者boss停驻的时候执行
        if (bossState == BossState.IDLE || ins.type == InstructionType.FIRE)
        {
            Instruction instruction = instructionQueue.Dequeue();
            Debug.Log("execute ins: " + ins.type);
            Execute(instruction);
        }
    }

    //执行一条指令
    void Execute(Instruction instruction)
    {
        if (instruction == null)
            return;
        switch (instruction.type)
        {
            case InstructionType.MOVEMENT:
                bossState = BossState.MOVING;
                moveController.MoveTo(instruction.targetPoint, instruction.speed);
                break;
            case InstructionType.DIVE:
                bossState = BossState.MOVING;
                moveController.DiveForPlayer();
                break;
            case InstructionType.STAY:
                bossState = BossState.STAY;
                moveController.stayingForSeconds(instruction.stayingTime);
                break;
            case InstructionType.FIRE:
                ExecuteFireInstruction(instruction);
                break;
        }
        if (loopStarted)
            historyQueue.Enqueue(instruction);
    }

    //射击指令比较复杂，单独使用一个函数处理
    void ExecuteFireInstruction(Instruction instruction)
    {
        switch (instruction.fireType)
        {
            case FireType.BUBBLE:
                shotController.FireBubbleBullets(instruction.bulletsCount);
                break;
            case FireType.LAZER:
                shotController.FireLazerBullets(instruction.countOfWaves);
                break;
            case FireType.ARC_FORMATION:
                shotController.FireArcFormationBullet(instruction.bulletsCount, instruction.unitAngle, instruction.countOfWaves);
                break;
            case FireType.RIGHT_FORMATION:
                shotController.FireRightAngleBullets(instruction.countOfWaves);
                break;
            case FireType.MISSILE:
                shotController.FireMissile();
                break;
        }
    }

    //方法说明见该类下方的IBossProxy接口的定义
    public void DiveForPlayer()
    {
        Instruction instruction = new Instruction();
        instruction.type = InstructionType.DIVE;
        instructionQueue.Enqueue(instruction);
    }

    public void FireArcFormationBullets(int count, int unitAngle, int countOfWaves)
    {
        Instruction instruction = new Instruction();
        instruction.type = InstructionType.FIRE;
        instruction.fireType = FireType.ARC_FORMATION;
        instruction.bulletsCount = count;
        instruction.unitAngle = unitAngle;
        instruction.countOfWaves = countOfWaves;
        instructionQueue.Enqueue(instruction);
    }

    public void FireBubbleBullets(int count)
    {
        Instruction instruction = new Instruction();
        instruction.type = InstructionType.FIRE;
        instruction.fireType = FireType.BUBBLE;
        instruction.bulletsCount = count;
        instructionQueue.Enqueue(instruction);
    }

    public void FireLazerBullets(int countOfWaves)
    {
        Instruction instruction = new Instruction();
        instruction.type = InstructionType.FIRE;
        instruction.fireType = FireType.LAZER;
        instruction.countOfWaves = countOfWaves;
        instructionQueue.Enqueue(instruction);
    }

    public void FireMissiles()
    {
        Instruction instruction = new Instruction();
        instruction.type = InstructionType.FIRE;
        instruction.fireType = FireType.MISSILE;
        instructionQueue.Enqueue(instruction);
    }

    public void FireRightFormationBullets(int countOfWaves)
    {
        Instruction instruction = new Instruction();
        instruction.type = InstructionType.FIRE;
        instruction.fireType = FireType.RIGHT_FORMATION;
        instruction.countOfWaves = countOfWaves;
        instructionQueue.Enqueue(instruction);
    }

    public void MoveTo(Vector3 targetPoint, float speed)
    {
        Instruction instruction = new Instruction();
        instruction.type = InstructionType.MOVEMENT;
        instruction.targetPoint = targetPoint;
        instruction.speed = speed;
        instructionQueue.Enqueue(instruction);
    }

    public void StayForSeconds(float seconds)
    {
        Instruction instruction = new Instruction();
        instruction.type = InstructionType.STAY;
        instruction.stayingTime = seconds;
        instructionQueue.Enqueue(instruction);
    }

    public void LoopStart()
    {
        instructionQueue.Enqueue(Instruction.LOOP_START);
    }

    public void LoopEnd()
    {
        instructionQueue.Enqueue(Instruction.LOOP_END);
    }

    public void RemoveAllInstructions(bool immediate)
    {
        if (immediate)
        {
            instructionQueue.Clear();
            historyQueue.Clear();
        }
        else
        {
            instructionQueue.Enqueue(Instruction.REMOVE_ALL);
        }
    }

    //当某一条移动或者停驻指令结束的时候会收到改回调，在这里面改变BossState
    public void OnBossMoveCompleted(bool changeState)
    {
        if (changeState)
            bossState = BossState.IDLE;
    }
}

// IBossProxy接口定义了可以在boss身上进行的操作。
public interface IBossProxy
{
    //发射呈直角队形的子弹，countOfWaves表示有几波子弹
    void FireRightFormationBullets(int countOfWaves);

    //发射弧线队形的子弹，count：数量，count * unitAngle = 弧线的圆心角，180°时就是半圆形，360°就是圆形
    void FireArcFormationBullets(int count, int unitAngle, int countOfWaves);

    //发射激光子弹
    void FireLazerBullets(int count);

    //发射导弹，boss不同导弹类型也不同
    void FireMissiles();

    //发射气泡子弹，有喷射的效果，运动方式在一定范围内随机
    void FireBubbleBullets(int count);

    //以特定速度移动到某一点
    void MoveTo(Vector3 targetPoint, float speed);

    //向玩家所在位置进行一次俯冲
    void DiveForPlayer();

    //待在现在的位置一段时间
    void StayForSeconds(float seconds);

    //指令循环开始
    //LoopStart()和LoopEnd()间的指令将会循环执行
    void LoopStart();

    //指令循环结束
    void LoopEnd();

    //取消当前指令队列(也包括historyQueue)中的所有指令
    //immediate为true表示立即执行，为false则放入指令队列等待执行
    void RemoveAllInstructions(bool immediate);

}

//代表一条指令
public class Instruction
{
    public InstructionType type;
    public Vector3 targetPoint = Vector3.zero;
    public float stayingTime = 0f;

    public float speed = 0f;
    public FireType fireType = FireType.NONE;

    public int bulletsCount = 20;

    public int unitAngle = 8;

    public int countOfWaves = 1;

    //预先构造的几条指令，方便使用
    public static Instruction LOOP_START = new Instruction(InstructionType.LOOP_START);

    public static Instruction LOOP_END = new Instruction(InstructionType.LOOP_END);

    public static Instruction REMOVE_ALL = new Instruction(InstructionType.REMOVE_ALL);

    public Instruction() { }

    public Instruction(InstructionType type)
    {
        this.type = type;
    }
}

public enum InstructionType
{
    MOVEMENT, FIRE, STAY, LOOP_START, LOOP_END, REMOVE_ALL, DIVE
}

// boss的状态，为IDLE的时候可以执行任意的指令，FIRE可以在任意的状态下执行(STAY或者MOVING)
public enum BossState
{
    IDLE, STAY, MOVING, FIRE
}

public enum FireType
{
    BUBBLE, RIGHT_FORMATION, ARC_FORMATION, LAZER, MISSILE, NONE
}
