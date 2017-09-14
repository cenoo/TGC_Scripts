using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/8/2 19:43:06
*功能：击败boss后出现的提示语
*/

public class MissionCompletedMovement : MonoBehaviour
{
    private Rigidbody2D body;
    public float speed;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.velocity = new Vector3(0, -1, 0) * speed;
    }

    private IEnumerator Move()
    {
        Vector3 movement = new Vector3(0, 0, -5) - transform.position;
        float time = movement.magnitude / speed;
        body.velocity = new Vector3(0, -1, 0) * speed;
        yield return new WaitForSeconds(time);
        body.velocity = new Vector3(0, 0, 0);
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.y - 0) < 0.11)
            body.velocity = new Vector3(0, 0, 0);
    }

}
