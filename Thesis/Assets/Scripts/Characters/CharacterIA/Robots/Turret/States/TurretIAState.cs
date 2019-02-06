using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAM.FSM;

public class TurretIAState : HSMState<TurretIAController.State, TurretIAController.Trigger>
{
    protected TurretIAController controller;

    public TurretIAState(TurretIAController.State state, string debugName) : base(state, debugName)
    {

    }

    protected override void OnEnter()
    {
        
    }

    protected override void OnExit()
    {
        
    }

    protected override void OnUpdate()
    {
        
    }
}
