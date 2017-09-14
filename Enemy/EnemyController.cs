using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/*
*作者：吴胜刚、罗钦、罗进、何东、金宁敏
*日期：2017/7/27 20:21:46
*功能：控制各个关卡的所有敌机（包括boss）的出现时机，这里的scenarioX()方法里的就是具体的关卡流程
*/

class EnemyController : MonoBehaviour
{
    //xxxPrefab类型的成员变量就是产生敌机需要使用的prefab资源
    public GameObject BossPrefab;

    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;

    public GameObject enemyPrefab4;

    public GameObject enemyPrefab5;

    public GameObject monsterPrefab;

    public GameObject sprayMonsterPrefab;

    //构造出来的敌机都将包含在这个gameObject下面，使用这个是为了消除缩放造成的大小问题
    public GameObject enemyParent;

    //指向构建出来的boss对应的gameObject
    private GameObject boss;

    //记录是否产生过boss，防止boss被消灭后再次产生boss的bug
    private bool hasShowedBoss = false;

    //boss产生的地点
    public static Vector3 bossBirthPlace = new Vector3(2.5f, 0, -5);

    //boss在关卡开始一段时间后出现，单位为秒
    public float bossShowTime;

    //记录关卡开始的时刻
    private float startTime;

    //当前的关卡等级，根据不同的关卡选择不同的 scenarioX()方法
    public int level;

    void Start()
    {
        startTime = Time.time;
        bossShowTime += startTime;
        switch (level)
        {
            case 1:
                StartCoroutine(scenario());
                break;
            case 2:
                StartCoroutine(scenario2());
                break;
        }
    }

    void Update()
    {

        if (Time.time >= bossShowTime && !hasShowedBoss)
        {
            ShowBoss();
        }

    }

    //第一关的关卡流程
    //关卡流程设计每个人负责大概30s长度，之后进行整合
    private IEnumerator scenario()
    {
        /***************************************************************Part1*****************************************************/
        //5s
        yield return new WaitForSeconds(5);
        GeneratorHelper.GeneratorGameObject(new Vector3(1.9f, 0.89f, -5), sprayMonsterPrefab, enemyParent);
        //从屏幕上方出现3个敌机
        ShowLineMovementEnemies(enemyPrefab1, 3, new Vector3(0.5f, 1.5f, -5), new Vector3(0.5f, 0, -5));
        //从屏幕下方出现3个敌机
        ShowLineMovementEnemies(enemyPrefab1, 3, new Vector3(1.0f, -1.5f, -5), new Vector3(1.0f, 0f, -5));
        //10s
        yield return new WaitForSeconds(5);
        //yield return new WaitForSeconds(2);
        //从屏幕右边直线进入3个敌机
        // ShowLineMovementEnemies(enemyPrefab1, 1, new Vector3(2.5f, 0.3f, -5), new Vector3(0, 0.3f, -5));
        // ShowLineMovementEnemies(enemyPrefab1, 1, new Vector3(2.5f, 0, -5), new Vector3(0, 0, -5));
        // ShowLineMovementEnemies(enemyPrefab1, 1, new Vector3(2.5f, -0.3f, -5), new Vector3(0, -0.3f, -5));

        ShowLineMovementEnemies(monsterPrefab, 1, new Vector3(2.5f, 0.3f, -5), new Vector3(0, 0.3f, -5));
        ShowLineMovementEnemies(monsterPrefab, 1, new Vector3(2.5f, 0, -5), new Vector3(0, 0, -5));
        ShowLineMovementEnemies(monsterPrefab, 1, new Vector3(2.5f, -0.3f, -5), new Vector3(0, -0.3f, -5));


        //15s
        yield return new WaitForSeconds(5);
        //上方 下方V字型出现
        ShowVeeMovementEnemies(enemyPrefab1, 3, new Vector3(2.5f, 2.5f, -5), new Vector3(0, 0, -5), 0.3f);
        ShowVeeMovementEnemies(enemyPrefab1, 3, new Vector3(2.5f, -2.5f, -5), new Vector3(0, 0, -5), 0.3f);
        //20s
        yield return new WaitForSeconds(5);
        //从右侧出现3个直线运动敌人 3个驻停敌人
        ShowLineMovementEnemies(enemyPrefab2, 1, new Vector3(2.5f, 0.3f, -5), new Vector3(0, 0.3f, -5));
        ShowLineMovementEnemies(enemyPrefab3, 1, new Vector3(2.5f, 0, -5), new Vector3(0, 0, -5));
        ShowLineMovementEnemies(enemyPrefab2, 1, new Vector3(2.5f, -0.3f, -5), new Vector3(0, -0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(2.5f, 0.3f, -5), new Vector3(0, 0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(2.5f, 0, -5), new Vector3(0, 0, -5));
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(2.5f, -0.3f, -5), new Vector3(0, -0.3f, -5));
        //25s
        yield return new WaitForSeconds(10);
        //从屏幕上方顺序进入3个敌机，然后从下方出去
        ShowLineMovementEnemies(enemyPrefab3, 5, new Vector3(2.5f, 0.2f, -5), new Vector3(0, 0.3f, -5));
        ShowLineMovementEnemies(enemyPrefab1, 5, new Vector3(2.5f, -0.2f, -5), new Vector3(0, -0.3f, -5));
        /***************************************************************Part2*****************************************************/
        yield return new WaitForSeconds(2);
        ShowLineMovementEnemies(enemyPrefab1, 3, new Vector3(2.5f, 0, -5),
            new Vector3(Random.Range(0f, 1.4f), Random.Range(-0.5f, 0.5f), -5));
        yield return new WaitForSeconds(3); //0-5s        
        ShowLineMovementEnemies(enemyPrefab2, 3, new Vector3(2.5f, 0.3f, -5),
            new Vector3(Random.Range(0f, 1.4f), Random.Range(-0.5f, 0.5f), -5));
        yield return new WaitForSeconds(2);
        ShowVeeMovementEnemies(enemyPrefab2, 3, new Vector3(Random.Range(0f, 1f), 1f, -5), new Vector3(0, 0, -5), 0.3f);
        ShowVeeMovementEnemies(enemyPrefab1, 3, new Vector3(Random.Range(0f, 1f), -1f, -5), new Vector3(0, 0, -5), 0.3f);

        yield return new WaitForSeconds(3); //5-10s
        ShowLineMovementEnemies(enemyPrefab1, 3, new Vector3(2.5f, 0, -5),
            new Vector3(Random.Range(0f, 1.4f), Random.Range(-0.5f, 0.5f), -5));
        yield return new WaitForSeconds(1);
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1f, 1f, -5), new Vector3(1, 0.6f, -5));
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(1f, 1f, -5), new Vector3(1, 0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1f, 1f, -5), new Vector3(1, 0, -5));
        //ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1f, 1f, -5), new Vector3(1, -0.3f, -5));
        //ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(0.7f, -1f, -5), new Vector3(0, 0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(0.7f, -1f, -5), new Vector3(0, 0, -5));
        ShowEnterAnchorEnemies(enemyPrefab3, 1, new Vector3(0.7f, -1f, -5), new Vector3(0, -0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(0.7f, -1f, -5), new Vector3(0, -0.6f, -5));
        //yield return new WaitForSeconds(4); //10-15s
        yield return new WaitForSeconds(1);
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.3f, 1f, -5), new Vector3(1.3f, 0.9f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.3f, 1f, -5), new Vector3(1.3f, 0.6f, -5));
        //ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.3f, 1f, -5), new Vector3(1.3f, 0.3f, -5));;
        //ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.3f, -1f, -5), new Vector3(0, -0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.3f, -1f, -5), new Vector3(0, -0.6f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.3f, -1f, -5), new Vector3(0, -0.9f, -5));
        yield return new WaitForSeconds(2);
        ShowVeeMovementEnemies(enemyPrefab3, 2, new Vector3(0, 1f, -5), new Vector3(0, 0, -5), 0.3f);
        ShowVeeMovementEnemies(enemyPrefab1, 2, new Vector3(0, -1f, -5), new Vector3(0, 0, -5), 0.3f);
        yield return new WaitForSeconds(2); //15-20s
        yield return new WaitForSeconds(3);
        ShowLineMovementEnemies(enemyPrefab1, 3, new Vector3(2.5f, 0, -5),
            new Vector3(0, 0, -5));
        yield return new WaitForSeconds(2); //20-25s
        ShowLineMovementEnemies(enemyPrefab3, 5, new Vector3(2.5f, 0, -5),
            new Vector3(Random.Range(0f, 1f), Random.Range(-1f, 1f), -5));
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(0.2f, 1f, -5), new Vector3(0, 0, -5), 0.3f);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(-0.2f, -1f, -5), new Vector3(0, 0, -5), 0.3f);
        /***************************************************************Part3*****************************************************/
        //从屏幕右边直线进入3个敌机
        ShowLineMovementEnemies(enemyPrefab1, 3, new Vector3(2.5f, 0.3f, -5), new Vector3(0, 0.3f, -5));
        yield return new WaitForSeconds(5);
        //从屏幕右边直线进入3个敌机
        ShowLineMovementEnemies(enemyPrefab2, 3, new Vector3(2.5f, 0.3f, -5), new Vector3(0, 0.3f, -5));
        //从屏幕右上角斜线进入3个敌机，然后在屏幕中间点（0，9，-5)停驻0.3s，后从屏幕右下方飞出去
        ShowVeeMovementEnemies(enemyPrefab1, 3, new Vector3(2.5f, 2.5f, -5), new Vector3(0, 0, -5), 0.3f);
        //移动方式同上一个，但是从右下方飞入，右上方飞出
        ShowVeeMovementEnemies(enemyPrefab1, 3, new Vector3(2.5f, -2.5f, -5), new Vector3(0, 0, -5), 0.3f);
        yield return new WaitForSeconds(3);
        //从屏幕右边直线进入3个敌机
        ShowLineMovementEnemies(enemyPrefab3, 3, new Vector3(2.5f, -0.3f, -5), new Vector3(0, 0.3f, -5));
        //从屏幕右侧水平成排进入4个敌人，然后停在屏幕中某一点（0.3f，2f，-5）
        ShowEnterAnchorEnemies(enemyPrefab1, 4, new Vector3(2.5f, 0.5f, -5), new Vector3(0.3f, 0.5f, -5));
        //移动方式同上一行，但是是对称的
        ShowEnterAnchorEnemies(enemyPrefab2, 2, new Vector3(2.5f, -0.5f, -5), new Vector3(0.3f, -0.5f, -5));
        yield return new WaitForSeconds(1);
        ShowEnterAnchorEnemies(enemyPrefab3, 2, new Vector3(2.5f, -0.5f, -5), new Vector3(0.3f, -0.5f, -5));

        yield return new WaitForSeconds(7);
        //从屏幕上方顺序进入3个敌机，然后从下方出去
        ShowLineMovementEnemies(enemyPrefab1, 3, new Vector3(1f, 2.5f, -5), new Vector3(1f, -2.5f, -5));
        //移动轨迹类似上一行代码，但是从下方进入，上方出去
        ShowLineMovementEnemies(enemyPrefab1, 3, new Vector3(0.5f, -2.5f, -5), new Vector3(0.5f, 2.5f, -5));

        yield return new WaitForSeconds(2);
        //从屏幕右上方进入一个敌机，从屏幕左下方离开
        ShowLineMovementEnemies(enemyPrefab1, 1, new Vector3(2f, 0.6f, -5), new Vector3(-0.5f, 0, -5));
        //从屏幕右下方进入一个敌机，从屏幕左上方离开
        ShowLineMovementEnemies(enemyPrefab2, 1, new Vector3(2f, -0.6f, -5), new Vector3(-0.5f, 0, -5));
        //从屏幕右侧正中进入一个敌人，沿直线从左侧退出
        ShowLineMovementEnemies(enemyPrefab3, 1, new Vector3(2f, 0, -5), new Vector3(0, 0, -5));
        yield return new WaitForSeconds(0.5f);
        //从左上方顺序进入四辆敌机，在屏幕中间转向，后从屏幕左下方离开
        ShowVeeMovementEnemies(enemyPrefab1, 4, new Vector3(0, 1f, -5), new Vector3(1, 0, -5), 0);
        //类似上一种敌机，但是移动轨迹对称
        ShowVeeMovementEnemies(enemyPrefab1, 4, new Vector3(0, -1, -5), new Vector3(1, 0, -5), 0);

        yield return new WaitForSeconds(5);
        //下面四句，都是产生一个从屏幕右侧进入的敌机，直线水平运动，但是四个是同时进入的
        ShowLineMovementEnemies(enemyPrefab1, 2, new Vector3(2f, 0.6f, -5), new Vector3(0, 0.6f, -5));
        ShowLineMovementEnemies(enemyPrefab2, 2, new Vector3(2f, 0.3f, -5), new Vector3(0, 0.3f, -5));
        ShowLineMovementEnemies(enemyPrefab2, 2, new Vector3(2f, -0.6f, -5), new Vector3(0, -0.6f, -5));
        ShowLineMovementEnemies(enemyPrefab1, 2, new Vector3(2f, -0.3f, -5), new Vector3(0, -0.3f, -5));

        yield return new WaitForSeconds(0.5f);
        //从屏幕上方依次进入四架敌机，沿垂直直线运动
        ShowLineMovementEnemies(enemyPrefab1, 4, new Vector3(0, 1f, -5), new Vector3(0, -1f, -5));
        //类似上一个，方向相反
        ShowLineMovementEnemies(enemyPrefab1, 4, new Vector3(0.1f, -1f, -5), new Vector3(0.1f, 1f, -5));
        /***************************************************************Part4*****************************************************/
        //--3个一列横排定点出现
        yield return new WaitForSeconds(5);
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(1.7f, 0.3f, -5), new Vector3(1.5f, 0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab3, 1, new Vector3(1.7f, 0f, -5), new Vector3(1.5f, 0f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.7f, -0.3f, -5), new Vector3(1.5f, -0.3f, -5));

        //--3个一列横排定点出现和两侧出现
        yield return new WaitForSeconds(5);
        //3个一列横排定点出现
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.7f, 0.3f, -5), new Vector3(1.5f, 0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.7f, 0f, -5), new Vector3(1.5f, 0f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.7f, -0.3f, -5), new Vector3(1.5f, -0.3f, -5));
        //两侧出现
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(1.5f, 0.9f, -5), new Vector3(1.5f, 0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(1.2f, 0.9f, -5), new Vector3(1.2f, 0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(0.9f, 0.9f, -5), new Vector3(0.9f, 0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(1.5f, -0.9f, -5), new Vector3(1.5f, -0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(1.2f, -0.9f, -5), new Vector3(1.2f, -0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(0.9f, -0.9f, -5), new Vector3(0.9f, -0.65f, -5), 3);

        //--2行出现
        yield return new WaitForSeconds(5);
        //4个一列横排出现
        ShowLineMovementEnemies(enemyPrefab2, 4, new Vector3(1.7f, 0.35f, -5), new Vector3(1.5f, 0.35f, -5));
        //4个一列横排出现
        ShowLineMovementEnemies(enemyPrefab3, 4, new Vector3(1.7f, -0.35f, -5), new Vector3(1.5f, -0.35f, -5));

        //--2列出现,一行从上之下，一行从下至上
        yield return new WaitForSeconds(5);
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.1f, 1.4f, -5), new Vector3(1.1f, 0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(1.1f, 1.1f, -5), new Vector3(1.1f, 0f, -5));
        ShowEnterAnchorEnemies(enemyPrefab3, 1, new Vector3(1.1f, 0.8f, -5), new Vector3(1.1f, -0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab3, 1, new Vector3(1.5f, -1.4f, -5), new Vector3(1.5f, -0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(1.5f, -1.1f, -5), new Vector3(1.5f, 0, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.5f, -0.8f, -5), new Vector3(1.5f, -0.3f, -5));

        //--3个一列横排定点出现
        yield return new WaitForSeconds(5);
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.7f, 0.3f, -5), new Vector3(1.5f, 0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.7f, 0f, -5), new Vector3(1.5f, 0f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.7f, -0.3f, -5), new Vector3(1.5f, -0.3f, -5));

        //--两侧出现
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(1.5f, 0.9f, -5), new Vector3(1.5f, 0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(1.2f, 0.9f, -5), new Vector3(1.2f, 0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(0.9f, 0.9f, -5), new Vector3(0.9f, 0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(1.5f, -0.9f, -5), new Vector3(1.5f, -0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(1.2f, -0.9f, -5), new Vector3(1.2f, -0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(0.9f, -0.9f, -5), new Vector3(0.9f, -0.65f, -5), 3);

    }

    //第二关的关卡流程
    private IEnumerator scenario2()
    {
        //5s
        yield return new WaitForSeconds(5);
        //从屏幕上方出现3个敌机
        ShowLineMovementEnemies(enemyPrefab2, 3, new Vector3(0.5f, 1.5f, -5), new Vector3(0.5f, 0, -5));
        //从屏幕下方出现3个敌机
        ShowLineMovementEnemies(enemyPrefab2, 3, new Vector3(1.0f, -1.5f, -5), new Vector3(1.0f, 0f, -5));
        //10s
        yield return new WaitForSeconds(5);
        //yield return new WaitForSeconds(2);
        //从屏幕右边直线进入2*2敌机
        ShowLineMovementEnemies(enemyPrefab2, 2, new Vector3(2.5f, 0.3f, -5), new Vector3(0, 0.3f, -5));
        ShowLineMovementEnemies(enemyPrefab2, 2, new Vector3(2.5f, 0, -5), new Vector3(0, 0, -5));
        //15s
        yield return new WaitForSeconds(5);
        //上方 下方V字型出现
        ShowVeeMovementEnemies(enemyPrefab2, 3, new Vector3(2.5f, 2.5f, -5), new Vector3(0, 0, -5), 0.3f);
        ShowVeeMovementEnemies(enemyPrefab2, 3, new Vector3(2.5f, -2.5f, -5), new Vector3(0, 0, -5), 0.3f);
        //20s
        yield return new WaitForSeconds(5);
        //从右侧出现3个直线运动敌人 3个驻停敌人
        ShowLineMovementEnemies(enemyPrefab2, 1, new Vector3(2.5f, 0.3f, -5), new Vector3(0, 0.3f, -5));
        ShowLineMovementEnemies(enemyPrefab2, 1, new Vector3(2.5f, 0, -5), new Vector3(0, 0, -5));
        ShowLineMovementEnemies(enemyPrefab2, 1, new Vector3(2.5f, -0.3f, -5), new Vector3(0, -0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(2.5f, 0.3f, -5), new Vector3(0, 0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(2.5f, 0, -5), new Vector3(0, 0, -5));
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(2.5f, -0.3f, -5), new Vector3(0, -0.3f, -5));
        //25s
        yield return new WaitForSeconds(10);
        //从屏幕上方顺序进入3个敌机，然后从下方出去
        ShowLineMovementEnemies(enemyPrefab2, 5, new Vector3(2.5f, 0.2f, -5), new Vector3(0, 0.3f, -5));
        ShowLineMovementEnemies(enemyPrefab2, 5, new Vector3(2.5f, -0.2f, -5), new Vector3(0, -0.3f, -5));
        /***************************************************************Part2*****************************************************/
        yield return new WaitForSeconds(2);
        ShowLineMovementEnemies(enemyPrefab1, 3, new Vector3(2.5f, 0, -5), new Vector3(Random.Range(0f, 1.4f), Random.Range(-0.5f, 0.5f), -5));
        yield return new WaitForSeconds(3); //0-5s        
        ShowLineMovementEnemies(enemyPrefab2, 3, new Vector3(2.5f, 0.3f, -5), new Vector3(Random.Range(0f, 1.4f), Random.Range(-0.5f, 0.5f), -5));
        yield return new WaitForSeconds(2);
        ShowVeeMovementEnemies(enemyPrefab3, 3, new Vector3(Random.Range(0f, 1f), 1f, -5), new Vector3(0, 0, -5), 0.3f);
        ShowVeeMovementEnemies(enemyPrefab3, 3, new Vector3(Random.Range(0f, 1f), -1f, -5), new Vector3(0, 0, -5), 0.3f);

        yield return new WaitForSeconds(3); //5-10s
        GeneratorHelper.GeneratorGameObject(new Vector3(1.9f, 0.89f, -5), sprayMonsterPrefab, enemyParent);

        ShowLineMovementEnemies(enemyPrefab2, 3, new Vector3(2.5f, 0, -5),
            new Vector3(Random.Range(0f, 1.4f), Random.Range(-0.5f, 0.5f), -5));
        yield return new WaitForSeconds(1);
        ShowLineMovementEnemies(enemyPrefab3, 1, new Vector3(1f, 1f, -5), new Vector3(1, 0.6f, -5));
        ShowLineMovementEnemies(enemyPrefab3, 1, new Vector3(1f, 1f, -5), new Vector3(1, 0.3f, -5));
        ShowLineMovementEnemies(enemyPrefab3, 1, new Vector3(1f, 1f, -5), new Vector3(1, 0, -5));
        yield return new WaitForSeconds(2);
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(1f, 1f, -5), new Vector3(1, -0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(0.7f, -1f, -5), new Vector3(0, 0.3f, -5));
        yield return new WaitForSeconds(1);
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.3f, 1f, -5), new Vector3(1.3f, 0.9f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.3f, 1f, -5), new Vector3(1.3f, 0.6f, -5));
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(1.3f, -1f, -5), new Vector3(0, -0.6f, -5), 0f);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(1.3f, -1f, -5), new Vector3(0, -0.9f, -5), 0f);
        yield return new WaitForSeconds(2);
        ShowVeeMovementEnemies(enemyPrefab3, 2, new Vector3(0, 1f, -5), new Vector3(0, 0, -5), 0.3f);
        ShowVeeMovementEnemies(enemyPrefab3, 2, new Vector3(0, -1f, -5), new Vector3(0, 0, -5), 0.3f);
        yield return new WaitForSeconds(5); //15-20s
        ShowLineMovementEnemies(enemyPrefab2, 3, new Vector3(2.5f, 0, -5), new Vector3(0, 0, -5));
        yield return new WaitForSeconds(2); //20-25s
        ShowLineMovementEnemies(enemyPrefab1, 5, new Vector3(2.5f, 0, -5), new Vector3(Random.Range(0f, 1f), Random.Range(-1f, 1f), -5));
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(0.2f, 1f, -5), new Vector3(0, 0, -5), 0.3f);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(-0.2f, -1f, -5), new Vector3(0, 0, -5), 0.3f);
        /***************************************************************Part3*****************************************************/
        //从屏幕右边直线进入3个敌机
        ShowLineMovementEnemies(enemyPrefab3, 3, new Vector3(2.5f, 0.3f, -5), new Vector3(0, 0.3f, -5));
        yield return new WaitForSeconds(5);
        //从屏幕右边直线进入3个敌机
        ShowLineMovementEnemies(enemyPrefab1, 3, new Vector3(2.5f, 0.3f, -5), new Vector3(0, 0.3f, -5));
        //从屏幕右上角斜线进入3个敌机，然后在屏幕中间点（0，9，-5)停驻0.3s，后从屏幕右下方飞出去
        ShowVeeMovementEnemies(enemyPrefab2, 3, new Vector3(2.5f, 2.5f, -5), new Vector3(0, 0, -5), 0.3f);
        //移动方式同上一个，但是从右下方飞入，右上方飞出
        ShowVeeMovementEnemies(enemyPrefab2, 3, new Vector3(2.5f, -2.5f, -5), new Vector3(0, 0, -5), 0.3f);
        yield return new WaitForSeconds(3);
        //从屏幕右边直线进入3个敌机
        ShowLineMovementEnemies(enemyPrefab1, 3, new Vector3(2.5f, -0.3f, -5), new Vector3(0, 0.3f, -5));
        //从屏幕右侧水平成排进入4个敌人，然后停在屏幕中某一点（0.3f，2f，-5）
        ShowEnterAnchorEnemies(enemyPrefab3, 4, new Vector3(2.5f, 0.5f, -5), new Vector3(0.3f, 0.5f, -5));
        //移动方式同上一行，但是是对称的
        ShowEnterAnchorEnemies(enemyPrefab3, 4, new Vector3(2.5f, -0.5f, -5), new Vector3(0.3f, -0.5f, -5));
        yield return new WaitForSeconds(7);
        //从屏幕上方顺序进入3个敌机，然后从下方出去
        ShowLineMovementEnemies(enemyPrefab2, 3, new Vector3(1f, 2.5f, -5), new Vector3(1f, -2.5f, -5));
        //移动轨迹类似上一行代码，但是从下方进入，上方出去
        ShowLineMovementEnemies(enemyPrefab1, 3, new Vector3(0.5f, -2.5f, -5), new Vector3(0.5f, 2.5f, -5));

        yield return new WaitForSeconds(2);
        //从屏幕右上方进入一个敌机，从屏幕左下方离开
        ShowLineMovementEnemies(enemyPrefab3, 1, new Vector3(2f, 0.6f, -5), new Vector3(-0.5f, 0, -5));
        //从屏幕右下方进入一个敌机，从屏幕左上方离开
        ShowLineMovementEnemies(enemyPrefab3, 1, new Vector3(2f, -0.6f, -5), new Vector3(-0.5f, 0, -5));
        //从屏幕右侧正中进入一个敌人，沿直线从左侧退出
        ShowLineMovementEnemies(enemyPrefab3, 1, new Vector3(2f, 0, -5), new Vector3(0, 0, -5));
        yield return new WaitForSeconds(0.5f);
        //从左上方顺序进入四辆敌机，在屏幕中间转向，后从屏幕左下方离开
        ShowVeeMovementEnemies(enemyPrefab1, 4, new Vector3(0, 1f, -5), new Vector3(1, 0, -5), 0);
        //类似上一种敌机，但是移动轨迹对称
        ShowVeeMovementEnemies(enemyPrefab1, 4, new Vector3(0, -1, -5), new Vector3(1, 0, -5), 0);

        yield return new WaitForSeconds(5);
        GeneratorHelper.GeneratorGameObject(new Vector3(1.9f, 0.89f, -5), sprayMonsterPrefab, enemyParent);
        
        //下面四句，都是产生一个从屏幕右侧进入的敌机，直线水平运动，但是四个是同时进入的
        ShowLineMovementEnemies(enemyPrefab1, 2, new Vector3(2f, 0.6f, -5), new Vector3(0, 0.6f, -5));
        ShowLineMovementEnemies(enemyPrefab2, 2, new Vector3(2f, 0.3f, -5), new Vector3(0, 0.3f, -5));
        ShowLineMovementEnemies(enemyPrefab2, 2, new Vector3(2f, -0.6f, -5), new Vector3(0, -0.6f, -5));
        ShowLineMovementEnemies(enemyPrefab3, 2, new Vector3(2f, -0.3f, -5), new Vector3(0, -0.3f, -5));

        yield return new WaitForSeconds(0.5f);
        //从屏幕上方依次进入四架敌机，沿垂直直线运动
        ShowLineMovementEnemies(enemyPrefab2, 3, new Vector3(0, 1f, -5), new Vector3(0, -1f, -5));
        //类似上一个，方向相反
        ShowLineMovementEnemies(enemyPrefab2, 3, new Vector3(0.1f, -1f, -5), new Vector3(0.1f, 1f, -5));
        /***************************************************************Part4*****************************************************/
        //--3个一列横排定点出现
        yield return new WaitForSeconds(5);
        ShowEnterAnchorEnemies(enemyPrefab3, 1, new Vector3(1.7f, 0.3f, -5), new Vector3(1.5f, 0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab3, 1, new Vector3(1.7f, 0f, -5), new Vector3(1.5f, 0f, -5));
        ShowEnterAnchorEnemies(enemyPrefab3, 1, new Vector3(1.7f, -0.3f, -5), new Vector3(1.5f, -0.3f, -5));

        //--3个一列横排定点出现和两侧出现
        yield return new WaitForSeconds(5);
        //3个一列横排定点出现
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.7f, 0.3f, -5), new Vector3(1.5f, 0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.7f, 0f, -5), new Vector3(1.5f, 0f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.7f, -0.3f, -5), new Vector3(1.5f, -0.3f, -5));
        //两侧出现
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(1.5f, 0.9f, -5), new Vector3(1.5f, 0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(1.2f, 0.9f, -5), new Vector3(1.2f, 0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab2, 1, new Vector3(0.9f, 0.9f, -5), new Vector3(0.9f, 0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab2, 1, new Vector3(1.5f, -0.9f, -5), new Vector3(1.5f, -0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab3, 1, new Vector3(1.2f, -0.9f, -5), new Vector3(1.2f, -0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab3, 1, new Vector3(0.9f, -0.9f, -5), new Vector3(0.9f, -0.65f, -5), 3);

        //--2行出现
        yield return new WaitForSeconds(5);
        //4个一列横排出现
        ShowLineMovementEnemies(enemyPrefab1, 4, new Vector3(1.7f, 0.35f, -5), new Vector3(1.5f, 0.35f, -5));
        //4个一列横排出现
        ShowLineMovementEnemies(enemyPrefab2, 4, new Vector3(1.7f, -0.35f, -5), new Vector3(1.5f, -0.35f, -5));

        //--2列出现,一行从上之下，一行从下至上
        yield return new WaitForSeconds(5);
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(1.1f, 1.4f, -5), new Vector3(1.1f, 0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(1.1f, 1.1f, -5), new Vector3(1.1f, 0f, -5));
        ShowEnterAnchorEnemies(enemyPrefab2, 1, new Vector3(1.1f, 0.8f, -5), new Vector3(1.1f, -0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.5f, -1.4f, -5), new Vector3(1.5f, -0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.5f, -1.1f, -5), new Vector3(1.5f, 0, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.5f, -0.8f, -5), new Vector3(1.5f, -0.3f, -5));

        //--3个一列横排定点出现
        yield return new WaitForSeconds(5);
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.7f, 0.3f, -5), new Vector3(1.5f, 0.3f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.7f, 0f, -5), new Vector3(1.5f, 0f, -5));
        ShowEnterAnchorEnemies(enemyPrefab1, 1, new Vector3(1.7f, -0.3f, -5), new Vector3(1.5f, -0.3f, -5));

        //--两侧出现
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(1.5f, 0.9f, -5), new Vector3(1.5f, 0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(1.2f, 0.9f, -5), new Vector3(1.2f, 0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab1, 1, new Vector3(0.9f, 0.9f, -5), new Vector3(0.9f, 0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab3, 1, new Vector3(1.5f, -0.9f, -5), new Vector3(1.5f, -0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab3, 1, new Vector3(1.2f, -0.9f, -5), new Vector3(1.2f, -0.65f, -5), 3);
        ShowVeeMovementEnemies(enemyPrefab3, 1, new Vector3(0.9f, -0.9f, -5), new Vector3(0.9f, -0.65f, -5), 3);
    }

    //显示boss
    private void ShowBoss()
    {
        boss = Instantiate(BossPrefab, bossBirthPlace, Quaternion.identity);
        hasShowedBoss = true;
        boss.transform.parent = GameObject.Find("BossContainer").transform;
        boss.transform.localScale = BossPrefab.transform.localScale;
    }

    //一下三个方法为创建并显示一个特定类型的敌人，包装后可以方便其他组员编写其各自的关卡逻辑

    //显示一个运动路线为直线的敌人（水平，垂直或者斜线）
    //根据birthPosition和direction可以确定运动的轨迹（两点确定一条直线）
    /* prefab: 构造敌人用到的prefab
    ** count： 显示的敌人的数量
    ** birthPosition：敌人出现的位置
    ** direction: 敌人运动的方向，与birthPosition共同确定
     */
    private void ShowLineMovementEnemies(GameObject prefab, int count, Vector3 birthPosition, Vector3 direction, float speed = 1f, BulletType type = BulletType.LAZER_BULLET)
    {
        MovementArg arg = new MovementArg();
        arg.positionArg = direction;
        arg.speed = speed;
        arg.bulletType = type;
        arg.type = MovementType.LINE_MOVEMENT;
        GenerateEnemyHelpr.GenerateLineEnemies(prefab, count, birthPosition, arg, enemyParent, this);
    }

    //显示一个一开始沿直线运动进入屏幕，然后在屏幕某一点停住的敌人
    /* prefab: 构造敌人用到的prefab
    ** count： 显示的敌人的数量
    ** birthPosition：敌人出现的位置
    ** anchorPoint：敌人停驻的位置
     */
    private void ShowEnterAnchorEnemies(GameObject prefab, int count, Vector3 birthPosition, Vector3 anchorPoint, float speed = 1f, BulletType type = BulletType.LAZER_BULLET)
    {
        MovementArg arg = new MovementArg();
        arg.positionArg = anchorPoint;
        arg.speed = speed;
        arg.bulletType = type;
        arg.type = MovementType.ENTER_ANCHOR_MOVEMENT;
        GenerateEnemyHelpr.GenerateEnterAnchorEnemies(prefab, count, birthPosition, arg, enemyParent, this);
    }

    //显示一个按照V字形运动的敌人：先直线进入屏幕，然后停住一段时间，然后再按照另一条直线离开屏幕
    /* prefab: 构造敌人用到的prefab
    ** count： 显示的敌人的数量
    ** birthPosition：敌人出现的位置
    ** turningPoint: 敌人停驻的位置
    ** anchorTime：敌人停驻的时间长短
     */
    private void ShowVeeMovementEnemies(GameObject prefab, int count, Vector3 birthPosition, Vector3 turningPoint, float anchorTime, float speed = 1f, BulletType type = BulletType.LAZER_BULLET)
    {
        MovementArg arg = new MovementArg();
        arg.positionArg = turningPoint;
        arg.anchorTime = anchorTime;
        arg.speed = speed;
        arg.bulletType = type;
        arg.type = MovementType.VEE_MOVEMENT;
        GenerateEnemyHelpr.GenerateVeeEnemies(prefab, count, birthPosition, arg, enemyParent, this);
    }
}
