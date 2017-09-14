using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者:罗钦
*日期:2017.8.7
*功能:发射激光敌人的集成行为
*/
public class SprayEnemy : MonoBehaviour {

    //开枪口
    public Transform weaponSlot;
    private float angle = 0;

    //初始位置和位移速度
    private Rigidbody2D body;
    private Vector3 initialPosition;
    public float speed;
    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();//得到属性
        StartCoroutine(moveAndShoot());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private IEnumerator moveAndShoot()
    {
        body.velocity = new Vector3(-1f, 0, -5) * speed;

        weaponSlot.GetComponent<Rigidbody2D>().velocity = new Vector3(-1f, 0, -5) * speed;//枪口跟随移动

        yield return new WaitForSeconds(2.2f);

        StartCoroutine(moveSpray(weaponSlot));//警示且扫描激光

        yield return new WaitForSeconds(4.8f);

        Destroy(weaponSlot.parent.gameObject);//超过预计时间边界摧毁他

        yield return 0;

    }

    private IEnumerator moveSpray(Transform weaponSlot) {
        //得到激光发射载体
        GameObject th = weaponSlot.GetChild(0).gameObject;
        //激光闪烁
        th.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        th.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        th.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        th.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        th.SetActive(true);
        //扫描激光
        for (float i = 135; i < 270f; i = i + Time.deltaTime*50)
        {
            weaponSlot.eulerAngles = new Vector3(0, 0, -i);
            yield return 0;
        }
        th.SetActive(false);

    }
}
