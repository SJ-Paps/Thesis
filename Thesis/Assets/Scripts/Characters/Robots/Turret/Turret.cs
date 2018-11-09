using SAM.FSM;
using UnityEngine;

public class Turret : Character
{
    [SerializeField]
    protected TurretAttackState attackState;

    [SerializeField]
    protected TurretMovingState movingState;

    protected override void Awake()
    {
        base.Awake();

        FSM<State, Trigger> movingFSM = new FSM<State, Trigger>();

        movingState.SetCharacterData(movingFSM, State.Moving, this, orders, blackboard);

        movingFSM.AddState(new TurretIdleState(movingFSM, State.Idle, this, orders, blackboard));
        movingFSM.AddState(movingState);

        movingFSM.MakeTransition(State.Idle, Trigger.Move, State.Moving);
        movingFSM.MakeTransition(State.Moving, Trigger.StopMoving, State.Idle);

        movingFSM.StartBy(State.Idle);

        FSM<State, Trigger> actionFSM = new FSM<State, Trigger>();

        attackState.SetCharacterData(actionFSM, State.Attacking, this, orders, blackboard);

        actionFSM.AddState(new TurretActionIdleState(actionFSM, State.Idle, this, orders, blackboard));
        actionFSM.AddState(attackState);
        

        actionFSM.MakeTransition(State.Idle, Trigger.Attack, State.Attacking);
        actionFSM.MakeTransition(State.Attacking, Trigger.StopAttacking, State.Idle);

        actionFSM.StartBy(State.Idle);

        AddStateMachineWhenAlive(movingFSM);
        AddStateMachineWhenAlive(actionFSM);
    }

    public override void GetEnslaved()
    {

    }

    protected override void OnFacingChanged(bool facingLeft)
    {
        
    }
}
