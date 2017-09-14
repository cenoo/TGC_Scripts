using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
*作者：吴胜刚
*日期：2017/7/27 13:25:01
*功能：辅助类，辅助构造不同运动方式和prefab的敌机对象
*/

public class GenerateEnemyHelpr
{
    public static void GenerateLineEnemies(GameObject prefabs, int count, Vector3 birthPosition, MovementArg arg, GameObject parent, MonoBehaviour behaviour)
    {
        behaviour.StartCoroutine(GenerateEnemies(prefabs, count, birthPosition, arg, parent, 0.3f));
    }


    public static void GenerateEnterAnchorEnemies(GameObject prefab, int count, Vector3 birthPosition, MovementArg arg, GameObject parent, MonoBehaviour behaviour)
    {
        behaviour.StartCoroutine(GenerateEnemies(prefab, count, birthPosition,arg, parent, 0.3f));
    }

    public static void GenerateVeeEnemies(GameObject prefab, int count, Vector3 birthPosition, MovementArg arg, GameObject parent, MonoBehaviour behaviour)
    {
        behaviour.StartCoroutine(GenerateEnemies(prefab, count, birthPosition, arg, parent, 0.3f));
    }

    private static IEnumerator GenerateEnemies(GameObject prefab, int count, Vector3 birthPosition, MovementArg arg, GameObject parent, float gapTime)
    {
        for (int i = 0; ;)
        {
            GameObject enemy = GeneratorHelper.GeneratorGameObject(birthPosition, prefab, parent);
            enemy.SendMessage("StartMove", arg);
            i++;
            if (i < count)
                yield return new WaitForSeconds(gapTime);
            else
                break;
        }
    }

}
