using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者:罗钦
*日期:2017.8.7
*功能:敌人激光扣血
*/
public class monsterlaser : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Debug.Log("laser!!!!!!!!!!!!!!!!!!!!!!");
            other.SendMessage("HitByBullet", 10);
        }
    }
}
