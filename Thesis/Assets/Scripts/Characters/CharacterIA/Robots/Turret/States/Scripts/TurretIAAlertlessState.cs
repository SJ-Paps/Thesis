﻿using System;
using UnityEngine;

public class TurretIAAlertlessState : TurretIAControllerHSMState
{
    private Action<Collider2D, Eyes> onTargetDetectedDelegate;

    public TurretIAAlertlessState(TurretIAController.State state, string debugName) : base(state, debugName)
    {
        onTargetDetectedDelegate = OnTargetDetected;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.Slave.GetEyes().onAnyStay += onTargetDetectedDelegate;
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.Slave.GetEyes().onAnyStay -= onTargetDetectedDelegate;
    }

    private void OnTargetDetected(Collider2D collider, Eyes eyes)
    {


        if(eyes.IsVisible(collider, Reg.walkableLayerMask, (1 << collider.gameObject.layer)))
        {
            SendEvent(TurretIAController.Trigger.SetFullAlert);
        }
    }
}
