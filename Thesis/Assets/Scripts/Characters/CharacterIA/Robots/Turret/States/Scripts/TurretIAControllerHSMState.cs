using UnityEngine;
using System;

public class TurretIAControllerHSMState : SJHSMState<TurretIAController.State, TurretIAController.Trigger, TurretIAController, TurretIAController.Blackboard>
{
    public TurretIAControllerHSMState(TurretIAController.State state, string debugName) : base(state, debugName)
    {

    }
}
