using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCasterMgr : IActorMgrInterface
{
    public string eventName;
    public bool active = false;
    public Vector3 offset = new Vector3(0, 0, 0.33f);

    private void Start()
    {
        if(actMgr == null)
        {
            actMgr = GetComponentInParent<ActorMgr>();
        }
    }
}
