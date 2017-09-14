using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/8/4 11:20:13
*功能：控制boss的血量，被击中时扣血
*/

public class BossHealthController : MonoBehaviour
{
    public GameObject missionCompleted;

    public GameObject explosion;

    private float fullBlood;

    private float halfBlood;

    private float quarterBlood;
    private bool underHalfBlood;

    private bool underQuarterBlood;

    private BossController bossController;

    public float blood;

    void Start()
    {
        bossController = GetComponent<BossController>();
        fullBlood = blood;
        halfBlood = fullBlood / 2;
        quarterBlood = fullBlood / 4;
    }

    //boss被击中时回调改方法
    void OnHit(float energy)
    {
        blood -= energy;
        if (blood < halfBlood && !underHalfBlood)
        {
            underHalfBlood = true;
            //半血时回调
            bossController.OnBossHalfBlood(blood);
        }
        else if (blood < quarterBlood && !underQuarterBlood)
        {
            underQuarterBlood = true;
            // 四分之一血时回调
            bossController.OnBossQuarterBlood(blood);
        }

        // 没血了销毁自身，弹出MissionCompleted提示
        if (blood <= 0)
        {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, Quaternion.identity);
            GameObject parent = GameObject.Find("EnemyController");
            GeneratorHelper.GeneratorGameObject(new Vector3(0f, 3f, -5f), missionCompleted, parent);
        }
    }
}
