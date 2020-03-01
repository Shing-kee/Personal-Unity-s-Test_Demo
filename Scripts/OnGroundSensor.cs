using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 测量Capsule的上下点，并判断消息发送
/// </summary>
public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capsol;
    public float offset = 0.1f;

    private Vector3 point1;
    private Vector3 point2;
    private float radius;

    // Start is called before the first frame update
    void Awake()
    {
        radius = capsol.radius - 0.05f;
    }

    void FixedUpdate()
    {
        point1 = transform.position + transform.up * (radius - offset);//(11.3,0,0)+(0,0.4,0)
        point2 = transform.position + transform.up * (capsol.height - offset) - transform.up * radius;//(11.3,0,0)+(0,1.9,0)-(0,0.5,0)

        Collider[] ouputcols = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Ground"));
        if (ouputcols.Length != 0)
        {
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }
    }
}
