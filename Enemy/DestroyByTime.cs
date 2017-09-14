using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/8/7 13:25:00
*功能：控制部分gameObject随着时间自动被销毁，当前用于爆炸的粒子效果的销毁
*/

public class DestroyByTime : MonoBehaviour
{
    public float seconds = 3;
    private float startTime;
    void Start()
    {
        startTime = Time.time;
    }

    void FixedUpdate()
    {
        if (Time.time - startTime > seconds)
            Destroy(gameObject);
    }
}
