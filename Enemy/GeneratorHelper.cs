using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
*作者：吴胜刚
*日期：2017/7/27 13:25:01
*功能：辅助类，最基础的构造gameObject的类，其他两个GenerateXX的辅助类都基于这个类
*/

public class GeneratorHelper:MonoBehaviour
{
    public static GameObject GeneratorGameObject(Vector3 position, GameObject prefab, GameObject parent)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        obj.transform.parent = parent.transform;
        obj.transform.localScale =prefab.transform.localScale;
        return obj;
    }
}