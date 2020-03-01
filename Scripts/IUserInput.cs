using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{

    [Header("===== Output Signals =====")]
    public float Dup;
    public float Dright;
    public float JUp;
    public float JRight;
    [Header("摇杆强度")]
    public float Dmag;
    [Header("目标方向")]
    public Vector3 Dvec;

    //1.pressing Signal
    public bool run;
    public bool defense;
    //2.trigger once signal
    public bool Action = false;
    public bool jump = false;
    protected bool lastJump;
    //public bool attack = false;
    protected bool lastAttack;
    public bool roll;
    public bool lockon;
    public bool lb, lt;
    public bool rb, rt;
    public bool lAttack, rAttack;
    public bool lPressA, rPressA;
    //3.double trigger

    [Header("===== Others =====")]
    public bool inputEnabled = true;

    protected float targetDup;
    protected float targetDright;
    protected float velocityDup;
    protected float velocityDright;

    /// <summary>
    /// Square to disc mapping(从平面转换圆型公式<解决人物沿斜边向量运动的问题>)
    /// </summary>
    /// <param name="input">原控制器方向的数值</param>
    /// <returns>返回值</returns>
    protected Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2);

        return output;
    }

    protected void UpdateDmagDvec(float curDup,float curDright)
    {
        Dmag = Mathf.Sqrt((curDup * curDup) + (curDright * curDright));//存储计算摇杆的移动向量/长度
        Dvec = curDup * transform.forward + curDright * transform.right;//存储计算人物方向向量
    }
}
