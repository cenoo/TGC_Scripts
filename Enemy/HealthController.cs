using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/8/5 16:21:46
*功能：控制敌机的血量，被击中是OnHit方法被调用，进行减血操作或者销毁操作
*/

public class HealthController : MonoBehaviour
{
    public float blood;

    public GameObject explosion;
    
    public void OnHit(float energy)
    {
        blood -= energy;

        if (blood <= 0)
        {
            Destroy(gameObject);
            GameObject obj = Instantiate(explosion, transform.position, Quaternion.identity, GameObject.Find("EnemyController").transform);
            obj.transform.localScale = explosion.transform.localScale;
            gameObject.SendMessage("SetBoom");
        }
    }
}
