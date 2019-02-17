using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHSMState : RobotHSMState
{
    protected new Turret character;

    public TurretHSMState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        character = (Turret)base.character;
    }
}