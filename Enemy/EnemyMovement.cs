using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*作者：吴胜刚
*日期：2017/8/4 17:53:46
*功能：控制敌机的运动，有直线运动，v字形运动和运动到一点后停止；
*当一架敌机被构建出来后，它的StartMove方法会被调用，根据传入的参数设置运动方式和运动速度
*/

public class EnemyMovement : MonoBehaviour
{

    private Rigidbody2D body;
    private Vector3 initialPosition;
    private EnemyShotController shotController;

    private bool hasEnterScreen = false;

    public float speed;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        shotController = GetComponent<EnemyShotController>();
        initialPosition = transform.position;
    }

    //一架敌机被构造出来后，该方法将会被调用，在里面设置敌机的移动参数
    void StartMove(MovementArg arg)
    {
        speed = arg.speed;
        shotController.setBulletType(arg.bulletType);
        switch (arg.type)
        {
            case MovementType.LINE_MOVEMENT:
                LineMovement(arg.positionArg);
                break;
            case MovementType.VEE_MOVEMENT:
                VeeMovement(arg.positionArg, arg.anchorTime);
                break;
            case MovementType.ENTER_ANCHOR_MOVEMENT:
                EnterAnchorMovement(arg.positionArg);
                break;

        }
    }

    //控制没有被击落的敌机的自动销毁，节约系统资源
    //当一架飞机先进入屏幕后又飞出了屏幕，则其需要被自动销毁。屏幕x坐标在区间（-1.7，1.7），y坐标在（-0.8，0.8）之间
    void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x) < 1.7 && Mathf.Abs(transform.position.y) < 0.8)
        {
            hasEnterScreen = true;
        }
        else if (hasEnterScreen || Mathf.Abs(transform.position.x) > 7 || Mathf.Abs(transform.position.y) > 5)
        {
            Destroy(gameObject);
        }
    }

    // 控制敌机进行直线运动
    void LineMovement(Vector3 direction)
    {
        Vector3 moveVector = (direction - initialPosition).normalized;
        body.velocity = moveVector * speed;
    }

    // 控制敌机进行V字形运动（使用协程控制，实际上的逻辑在VeeMovementImpl()中
    void VeeMovement(Vector3 turningPoint, float anchorTime)
    {
        StartCoroutine(VeeMovementImpl(turningPoint, anchorTime));
    }

    //v字形运动，使用协程的方式实现
    //turningPoint: 转折点，即v字的尖端那一个点的位置
    //anchorTime：停留时间，飞机运动到转折点时，可以在那个位置停留一段时间，之后再反向飞出
    private IEnumerator VeeMovementImpl(Vector3 turningPoint, float anchorTime)
    {
        Vector3 moveVector = turningPoint - initialPosition;
        float distance = moveVector.magnitude;
        float movingTime = distance / speed;
        moveVector = moveVector.normalized;
        body = GetComponent<Rigidbody2D>();
        body.velocity = moveVector * speed;
        yield return new WaitForSeconds(movingTime);
        body.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(anchorTime);
        moveVector.x = -moveVector.x;
        body.velocity = moveVector * speed;
    }

    //运动轨迹：进入后停止
    void EnterAnchorMovement(Vector3 anchorPoint)
    {
        StartCoroutine(EnterAnchorMovementImpl(anchorPoint));
    }

    //进入后停驻运动方式
    //anchorPoint:停驻点，飞机运动到这里时将永远停止在这里，直到被消灭
    IEnumerator EnterAnchorMovementImpl(Vector3 anchorPoint)
    {
        Vector3 moveVector = anchorPoint - initialPosition;
        float distance = moveVector.magnitude;
        float movingTime = distance / speed;
        moveVector = moveVector.normalized;
        body = GetComponent<Rigidbody2D>();
        body.velocity = moveVector * speed;
        yield return new WaitForSeconds(movingTime);
        body.velocity = new Vector3(0, 0, 0);
    }
}

//传给StartMove方法使用的运动相关的参数
public class MovementArg
{
    public Vector3 positionArg;
    public MovementType type;

    public float anchorTime;

    public float speed = 1;
    public BulletType bulletType = BulletType.LAZER_BULLET;
}

public enum MovementType
{
    LINE_MOVEMENT, VEE_MOVEMENT, ENTER_ANCHOR_MOVEMENT
}
