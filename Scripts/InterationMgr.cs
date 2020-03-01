using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterationMgr : IActorMgrInterface
{
    private CapsuleCollider interCol;

    public List<EventCasterMgr> lst_ovrlapEvnCstr = new List<EventCasterMgr>();

    void Start()
    {
        interCol = GetComponent<CapsuleCollider>();    
    }

    private void OnTriggerStay(Collider other)
    {
        EventCasterMgr[] evnCstrMgrs = other.GetComponents<EventCasterMgr>();
        foreach (var item in evnCstrMgrs)
        {
            if (!lst_ovrlapEvnCstr.Contains(item))
            {
                lst_ovrlapEvnCstr.Add(item);                
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        EventCasterMgr[] eventCasterMgrs = other.GetComponents<EventCasterMgr>();
        foreach (var item in eventCasterMgrs)
        {
            if (lst_ovrlapEvnCstr.Contains(item))
            {
                lst_ovrlapEvnCstr.Remove(item);
            }
        }
    }
}
