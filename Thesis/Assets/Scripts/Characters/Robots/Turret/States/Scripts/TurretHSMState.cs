using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHSMState : CharacterHSMState
{
    public new Turret Owner { get; protected set; }
    protected new Turret.Blackboard Blackboard { get; private set; }

    public TurretHSMState(byte stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        Owner = (Turret)base.Owner;
        Blackboard = (Turret.Blackboard)base.Blackboard;
    }
}