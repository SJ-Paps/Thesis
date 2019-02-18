using UnityEngine;
using System;

public class TurretIAControllerHSMState : IAControllerHSMState<Turret, TurretIAController.State, TurretIAController.Trigger, TurretIAController, TurretIAController.Blackboard>
{
    protected TurretIAController controller;

    public TurretIAControllerHSMState(TurretIAController.State state, string debugName) : base(state, debugName)
    {

    }
}
