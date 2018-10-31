using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAM.FSM;

public class Turret : Character
{
    protected override void Awake()
    {
        base.Awake();

        FSM<State, Trigger> movingFSM = new FSM<State, Trigger>();

        movingFSM.AddState(new TurretIdleState(movingFSM, State.Idle, this, orders, blackboard));
        movingFSM.AddState(new TurretMovingState(movingFSM, State.Moving, this, orders, blackboard));

        movingFSM.MakeTransition(State.Idle, Trigger.Move, State.Moving);
        movingFSM.MakeTransition(State.Moving, Trigger.StopMoving, State.Idle);

        movingFSM.StartBy(State.Idle);

        AddStateMachineWhenAlive(movingFSM);
    }

    public override void GetEnslaved()
    {
        
    }

    protected override void OnFacingChanged(bool facingLeft)
    {
        
    }
}
