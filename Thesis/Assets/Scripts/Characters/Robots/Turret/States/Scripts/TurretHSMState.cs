using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHSMState : RobotHSMState
{
    protected new Turret Owner { get; set; }

    public TurretHSMState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        Owner = (Turret)base.Owner;
    }
}