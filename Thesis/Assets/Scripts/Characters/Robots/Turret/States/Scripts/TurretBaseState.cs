using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBaseState : TurretHSMState
{
    public TurretBaseState(Turret.State stateId, string debugName = null) : base(stateId, debugName)
    {
    }
}
