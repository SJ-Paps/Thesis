using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAM.FSM;

public class TurretIAState : State<TurretIAController.State, TurretIAController.Trigger>
{
    protected TurretIAController.Blackboard blackboard;
    protected TurretIAController controller;

    public TurretIAState() : base(null, default(TurretIAController.State))
    {

    }

    public virtual void InitializeState(FSM<TurretIAController.State, TurretIAController.Trigger> stateMachine, TurretIAController.State state, TurretIAController controller, TurretIAController.Blackboard blackboard)
    {
        this.blackboard = blackboard;
        this.controller = controller;
        this.stateMachine = stateMachine;
        this.InnerState = state;
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
