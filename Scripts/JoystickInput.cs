using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摇杆参数
/// </summary>
public class JoystickInput : IUserInput
{
    [Header("===== Joystick Settings=====")]
    public string axisX = "axisX";
    public string axisY = "axisY";
    public string axis3 = "axis3";
    public string axis5 = "axis5";
    public string btnA = "btn0";
    public string btnB = "btn1";
    public string btnC = "btn2";
    public string btnD = "btn3";
    public string btnLB = "btn4";
    public string btnLT = "btn6";
    public string btnRB = "btn5";
    public string btnRT = "btn7";
    public string btnstick = "btn11";

    public MyButton buttonA = new MyButton();
    public MyButton buttonB = new MyButton();
    public MyButton buttonC = new MyButton();
    public MyButton buttonD = new MyButton();
    public MyButton buttonLB = new MyButton();
    public MyButton buttonLT = new MyButton();
    public MyButton buttonRB = new MyButton(); 
    public MyButton buttonRT = new MyButton(); 
    public MyButton buttonJstick = new MyButton();

    void Update()
    {
        buttonA.Tick(Input.GetButton(btnA));
        buttonB.Tick(Input.GetButton(btnB));
        buttonC.Tick(Input.GetButton(btnC));
        buttonD.Tick(Input.GetButton(btnD));
        buttonLB.Tick(Input.GetButton(btnLB));
        buttonLT.Tick(Input.GetButton(btnLT));
        buttonRB.Tick(Input.GetButton(btnRB));
        buttonRT.Tick(Input.GetButton(btnRT));
        buttonJstick.Tick(Input.GetButton(btnstick));

        targetDup = Input.GetAxis(axisY) ;
        targetDright = Input.GetAxis(axisX);

        if (inputEnabled == false)
        {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);//SmoothDamp用于缓慢变值
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);//ref velocity...暂存数据

        JUp = -1 * Input.GetAxis(axis5);
        JRight = Input.GetAxis(axis3);

        Vector2 tempAxis = SquareToCircle(new Vector2(Dright, Dup));//储存转换公式后的数据
        float curDright = tempAxis.x;
        float curDup = tempAxis.y;

        //人物方向（子物体）及人物移动[以X、Y轴为水平线移动]
        Dmag = Mathf.Sqrt((curDup * curDup) + (curDright * curDright));//存储计算摇杆的移动向量/长度
        Dvec = curDup * transform.forward + curDright * transform.right;//存储计算人物方向向量

        run = Input.GetButton(btnA);
        defense = Input.GetButton(btnLB);
        roll = buttonB.OnReleased && buttonB.IsDelaying;

        rb = buttonRB.OnPressed;
        rt = buttonRT.OnPressed;
        lb = buttonLB.OnPressed;
        lt = buttonLT.OnPressed;
        defense = buttonLB.IsPressing;
        lockon = buttonJstick.OnPressed;

        //bool newJump = Input.GetKey(btnB);
        //if (newJump != lastJump && newJump == true)
        //{
        //    jump = true;
        //}
        //else//现有问题：一旦按下去第一帧jump=true，但由于下一帧判断跳到else，使jump=false，导致Inspector面板jump没有打勾
        //{
        //    jump = false;
        //}
        //lastJump = newJump;

        //bool newAttack = Input.GetKey(btnC);
        //if (newAttack != lastAttack && newAttack == true)
        //{
        //    attack = true;
        //}
        //else
        //{
        //    attack = false;
        //}
        //lastAttack = newAttack;
    }
}
