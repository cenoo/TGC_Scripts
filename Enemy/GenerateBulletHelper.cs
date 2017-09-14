using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/7/27 13:25:01
*功能：辅助类，辅助构造子弹对象
*/

public class GenerateBulletHelper : MonoBehaviour {
	public static GameObject GenerateBullets(GameObject prefab, Vector3 position, GameObject parent)
	{
		return GeneratorHelper.GeneratorGameObject(position, prefab, parent);
	}
}
