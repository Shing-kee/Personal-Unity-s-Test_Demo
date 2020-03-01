using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 键盘参数
/// </summary>
public class KeyboardInput : IUserInput
{
    [Header("===== Keys Settings =====")]
    public string keyUp;
    public string keyDown;
    public string keyLeft;
    public string keyRight;

    public string keyLAttack;
    public string keyRAttack;

    public string keyJumpRoll;
    public string keyRun;
    public string keyDefense;

    public string keyJUp;
    public string keyJDown;
    public string keyJRight;
    public string keyJLeft;
    public string keyStick;

    public string keyTest;

    [Header("===== Mouse settings=====")]
    public bool mouseInabled = false;
    public float mouseSensitivityX = 1.0f;
    public float mouseSensitivityY = 1.0f;

    public MyButton buttonRun = new MyButton();
    public MyButton buttonRoll = new MyButton();
    public MyButton buttonLAttack = new MyButton();
    public MyButton buttonRAttack = new MyButton();
    public MyButton buttonDefense = new MyButton();
    public MyButton buttonCameraUp = new MyButton();
    public MyButton buttonCameraDown = new MyButton();
    public MyButton buttonCameraLeft = new MyButton();
    public MyButton buttonCameraRight = new MyButton();
    public MyButton buttonCamerastick = new MyButton();
    public MyButton buttonMsgEvent = new MyButton();

    void Update()
    { 
        buttonLAttack.Tick(Input.GetKey(keyLAttack));
        buttonRAttack.Tick(Input.GetKey(keyRAttack));
        buttonRoll.Tick(Input.GetKey(keyJumpRoll));
        buttonRun.Tick(Input.GetKey(keyRun));
        buttonDefense.Tick(Input.GetKey(keyDefense));
        buttonCameraUp.Tick(Input.GetKey(keyJUp));
        buttonCameraDown.Tick(Input.GetKey(keyJDown));
        buttonCameraLeft.Tick(Input.GetKey(keyJLeft));
        buttonCameraRight.Tick(Input.GetKey(keyJRight));
        buttonCamerastick.Tick(Input.GetKey(keyStick));
        buttonMsgEvent.Tick(Input.GetKey(keyTest));

        targetDup = (Input.GetKey(keyUp)? 1.0f : 0) - (Input.GetKey(keyDown)? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        if (!inputEnabled)
        {
            targetDup = 0;
            targetDright = 0;
        }

        if (mouseInabled)
        {
            JUp = Input.GetAxis("Mouse Y") * mouseSensitivityY;
            JRight = Input.GetAxis("Mouse X") * mouseSensitivityX;
        }
        else
        {
            JUp = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);
            JRight = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);//SmoothDamp用于缓慢变值
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);//ref velocity...暂存数据

        Vector2 tempAxis = SquareToCircle(new Vector2(Dright, Dup));//储存转换公式后的数据
        float curDright = tempAxis.x;
        float curDup = tempAxis.y;

        //人物方向（子物体）及人物移动[以X、Y轴为水平线移动]
        Dmag = Mathf.Sqrt((curDup * curDup) + (curDright * curDright));//存储计算摇杆的移动向量/长度
        Dvec = curDup * transform.forward + curDright * transform.right;//存储计算人物方向向量

        lAttack = buttonLAttack.IsPressing;
        rAttack = buttonRAttack.IsPressing;
        lPressA = buttonLAttack.OnReleased && buttonLAttack.IsDelaying;
        rPressA = buttonRAttack.OnPressed && !buttonRAttack.IsDelaying;
        defense = buttonDefense.IsPressing && !buttonLAttack.IsDelaying;
        lockon = buttonCamerastick.OnPressed;

        run = (buttonRun.IsPressing && !buttonRun.IsDelaying) || buttonRun.IsExtending;//从按下开始确定延迟时间后还在按则触发，放掉后时间内确定没有再按则取消
        jump = buttonRoll.OnPressed && buttonRoll.IsExtending;//从按下开始在拓展时间内再次检测到有jump才跳，比如时间内长按、连续按两次等
        roll = buttonRoll.OnReleased && buttonRoll.IsDelaying;

        Action = buttonMsgEvent.OnPressed;
    }
}
