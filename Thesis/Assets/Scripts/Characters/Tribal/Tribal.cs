using SAM.FSM;
using UnityEngine;


public abstract class Tribal : Character
{
    [SerializeField]
    protected CollectableObject currentCollectableObject;

    public CollectableObject CurrentCollectableObject
    {
        get
        {
            return currentCollectableObject;
        }

        protected set
        {
            currentCollectableObject = value;
        }
    }

    [SerializeField]
    protected TribalIdleState idleState;

    [SerializeField]
    protected TribalMovingState movingState;

    [SerializeField]
    protected TribalSlowDownState slowDownState;

    [SerializeField]
    protected TribalGroundedState groundedState;

    [SerializeField]
    protected TribalJumpingState jumpingState;

    [SerializeField]
    protected TribalFallingState fallingState;

    [SerializeField]
    protected TribalActionsIdleState actionIdleState;

    [SerializeField]
    protected TribalHiddenState hiddenState;

    [SerializeField]
    protected TribalPushingObjectState pushingState;

    protected FSM<State, Trigger> movementFSM, jumpingFSM, actionFSM;
    

    

    protected override void Awake()
    {
        base.Awake();

        if(CurrentCollectableObject != null)
        {
            CurrentCollectableObject.Collect(this);
        }

        movementFSM = new FSM<State, Trigger>();

        idleState.InitializeState(movementFSM, State.Idle, this, orders, blackboard);
        movingState.InitializeState(movementFSM, State.Moving, this, orders, blackboard);
        slowDownState.InitializeState(movementFSM, State.SlowingDown, this, orders, blackboard);

        movementFSM.AddState(idleState);
        movementFSM.AddState(movingState);
        movementFSM.AddState(slowDownState);

        movementFSM.MakeTransition(State.Idle, Trigger.Move, State.Moving);
        movementFSM.MakeTransition(State.Moving, Trigger.StopMoving, State.SlowingDown);
        movementFSM.MakeTransition(State.SlowingDown, Trigger.StopMoving, State.Idle);
        movementFSM.MakeTransition(State.SlowingDown, Trigger.Move, State.Moving);

        movementFSM.StartBy(State.Idle);

        jumpingFSM = new FSM<State, Trigger>();

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

        actionFSM = new FSM<State, Trigger>();

        actionIdleState.InitializeState(actionFSM, State.Idle, this, orders, blackboard, jumpingFSM, movementFSM);
        pushingState.InitializeState(actionFSM, State.Pushing, this, orders, blackboard);
        hiddenState.InitializeState(actionFSM, State.Hidden, this, orders, blackboard, jumpingFSM, movementFSM);

        actionFSM.AddState(actionIdleState);
        actionFSM.AddState(pushingState);
        actionFSM.AddState(hiddenState);

        actionFSM.MakeTransition(State.Idle, Trigger.Hide, State.Hidden);
        actionFSM.MakeTransition(State.Hidden, Trigger.StopHiding, State.Idle);

        actionFSM.MakeTransition(State.Idle, Trigger.Push, State.Pushing);
        actionFSM.MakeTransition(State.Pushing, Trigger.StopPushing, State.Idle);

        actionFSM.StartBy(State.Idle);

        AddStateMachineWhenAlive(movementFSM);
        AddStateMachineWhenAlive(jumpingFSM);
        AddStateMachineWhenAlive(actionFSM);
    }
}
