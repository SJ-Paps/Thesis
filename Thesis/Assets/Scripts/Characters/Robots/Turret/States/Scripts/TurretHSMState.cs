using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHSMState : CharacterHSMState<Turret.State, Turret, Turret.Blackboard>
{
    public TurretHSMState(Turret.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }
}