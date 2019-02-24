using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class XenophobicIAAlertlessState : XenophobicIAState
{
    private Action<Collider2D, Eyes> onSomethingDetectedDelegate;
    
    private float previousHiddenFindProbability;

    public XenophobicIAAlertlessState(XenophobicIAController.State state, string debugName) : base(state, debugName)
    {
        onSomethingDetectedDelegate += AnalyzeDetection;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.Slave.GetEyes().onAnyStay += onSomethingDetectedDelegate;
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.Slave.GetEyes().onAnyStay -= onSomethingDetectedDelegate;
    }

    private void AnalyzeDetection(Collider2D collider, Eyes eyes)
    {
        if (eyes.IsVisible(collider, Reg.walkableLayerMask, (1 << collider.gameObject.layer)))
        {
            SendEvent(XenophobicIAController.Trigger.GetAware);
        }
    }
}
