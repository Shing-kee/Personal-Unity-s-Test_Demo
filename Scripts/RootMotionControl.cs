using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 美术动画处理
/// </summary>
public class RootMotionControl : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnAnimatorMove()//一旦调用，则动画面板里的打勾选项将自动被官方隐藏
    {
        SendMessageUpwards("OnUpdateRootMotion",anim.deltaPosition);
    }
}
