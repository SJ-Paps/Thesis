using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDeadState : TurretHSMState
{
    public TurretDeadState(Turret.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }
}