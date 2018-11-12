using UnityEngine;
using SAM.FSM;

public abstract class Tribal : Character
{
    [SerializeField]
    protected TribalIdleState idleState;

    [SerializeField]
    protected TribalMovingState movingState;

    [SerializeField]
    protected TribalBrakeState brakeState;

    [SerializeField]
    protected TribalGroundedState groundedState;

    [SerializeField]
    protected TribalJumpingState jumpingState;

    [SerializeField]
    protected TribalFallingState fallingState;

    [SerializeField]
    protected TribalPushingObjectState pushingState;

    [SerializeField]
    protected TribalHiddenState hiddenState;

    [SerializeField]
    protected TribalActionsIdleState actionIdleState;

    protected override void Awake()
    {
        base.Awake();

        FSM<State, Trigger> movementFSM = new FSM<State, Trigger>();

        idleState.InitializeState(movementFSM, State.Idle, this, orders, blackboard);
        movingState.InitializeState(movementFSM, State.Moving, this, orders, blackboard);
        brakeState.InitializeState(movementFSM, State.SlowingDown, this, orders, blackboard);


        movementFSM.AddState(idleState);
        movementFSM.AddState(movingState);
        movementFSM.AddState(brakeState);

        movementFSM.MakeTransition(State.Idle, Trigger.Move, State.Moving);
        movementFSM.MakeTransition(State.Moving, Trigger.StopMoving, State.SlowingDown);
        movementFSM.MakeTransition(State.SlowingDown, Trigger.Move, State.Moving);
        movementFSM.MakeTransition(State.SlowingDown, Trigger.StopMoving, State.Idle);

        movementFSM.StartBy(State.Idle);

        FSM<State, Trigger> jumpingFSM = new FSM<State, Trigger>();

        groundedState.InitializeState(jumpingFSM, State.Grounded, this, orders, blackboard);
        jumpingState.InitializeState(jumpingFSM, State.Jumping, this, orders, blackboard);
        fallingState.InitializeState(jumpingFSM, State.Falling, this, orders, blackboard);


        jumpingFSM.AddState(groundedState);
        jumpingFSM.AddState(jumpingState);
        jumpingFSM.AddState(fallingState);

        jumpingFSM.MakeTransition(State.Grounded, Trigger.Jump, State.Jumping);
        jumpingFSM.MakeTransition(State.Grounded, Trigger.Fall, State.Falling);
        jumpingFSM.MakeTransition(State.Jumping, Trigger.Fall, State.Falling);
        jumpingFSM.MakeTransition(State.Falling, Trigger.Ground, State.Grounded);

        jumpingFSM.StartBy(State.Falling);

        FSM<State, Trigger> actionsFSM = new FSM<State, Trigger>();


        pushingState.InitializeState(actionsFSM, State.Pushing, this, orders, blackboard);
        hiddenState.InitializeState(actionsFSM, State.Hidden, this, orders, blackboard, jumpingFSM, movementFSM);
        actionIdleState.InitializeState(actionsFSM, State.Idle, this, orders, jumpingFSM, movementFSM, blackboard);

        actionsFSM.AddState(hiddenState);
        actionsFSM.AddState(pushingState);
        actionsFSM.AddState(actionIdleState);

        actionsFSM.MakeTransition(State.Idle, Trigger.Hide, State.Hidden);
        actionsFSM.MakeTransition(State.Hidden, Trigger.StopHiding, State.Idle);

        actionsFSM.MakeTransition(State.Idle, Trigger.Push, State.Pushing);
        actionsFSM.MakeTransition(State.Pushing, Trigger.StopPushing, State.Idle);

        actionsFSM.StartBy(State.Idle);

        AddStateMachineWhenAlive(movementFSM);
        AddStateMachineWhenAlive(jumpingFSM);
        AddStateMachineWhenAlive(actionsFSM);
    }
}
