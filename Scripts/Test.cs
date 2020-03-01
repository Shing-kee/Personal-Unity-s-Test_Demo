using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : ScrollRect
{
    //private bool isState = false;

    //private void Update()
    //{
    //    if (isState)
    //    {
    //        Debug.Log("Update!");
    //    }
    //}
    //private void OnBecameVisible()
    //{
    //    Debug.Log("Appear!");
    //    isState = true;
    //}
    //private void OnBecameInvisible()
    //{
    //    Debug.Log("Disappear!");
    //    isState = false;
    //}

    protected float mRadius = 0f;
    public RectTransform key;
    protected override void Start()
    {
        base.Start();
        //计算摇杆半径
        mRadius = (transform as RectTransform).sizeDelta.x * 0.5f;
        key = transform.Find("joystick").GetComponent<RectTransform>();//
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        var contentPosition = this.content.anchoredPosition;
        Debug.Log((key.position.x - transform.position.x) / 42);
        Debug.LogWarning((key.position.y - transform.position.y) / 42);
        if (contentPosition.magnitude > mRadius)
        {
            contentPosition = contentPosition.normalized * mRadius;
            SetContentAnchoredPosition(contentPosition);

        }
    }
}
