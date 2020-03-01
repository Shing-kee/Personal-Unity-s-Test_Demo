using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画触发器处理中心
/// </summary>
public class TriggerControl : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ResetTrigger(string triggerName)
    {
        //print()
        anim.ResetTrigger(triggerName);
    }
}
