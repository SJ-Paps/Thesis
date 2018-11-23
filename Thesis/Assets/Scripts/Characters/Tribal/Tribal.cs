using SAM.FSM;
using UnityEngine;
using System;


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

    [SerializeField]
    protected TribalGrapplingState grapplingState;

    protected FSM<State, Trigger> movementFSM, jumpingFSM, actionFSM;

    public override bool CanMove
    {
        get
        {
            return actionFSM.CurrentState.InnerState != State.Hidden && actionFSM.CurrentState.InnerState != State.Grappling;
        }
    }


    protected override void Awake()
    {
        base.Awake();

        if(CurrentCollectableObject != null)
        {
            CurrentCollectableObject.Collect(this);
        }

        movementFSM = new FSM<State, Trigger>();
        jumpingFSM = new FSM<State, Trigger>();
        actionFSM = new FSM<State, Trigger>();

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

        groundedState.InitializeState(jumpingFSM, State.Grounded, this, orders, blackboard);
        jumpingState.InitializeState(jumpingFSM, State.Jumping, this, orders, blackboard);
        fallingState.InitializeState(jumpingFSM, State.Falling, this, orders, blackboard);
        grapplingState.InitializeState(jumpingFSM, State.Grappling, this, orders, blackboard);


        jumpingFSM.AddState(groundedState);
        jumpingFSM.AddState(jumpingState);
        jumpingFSM.AddState(fallingState);
        jumpingFSM.AddState(grapplingState);

        jumpingFSM.MakeTransition(State.Grounded, Trigger.Jump, State.Jumping);
        jumpingFSM.MakeTransition(State.Grounded, Trigger.Fall, State.Falling);
        jumpingFSM.MakeTransition(State.Jumping, Trigger.Fall, State.Falling);
        jumpingFSM.MakeTransition(State.Falling, Trigger.Ground, State.Grounded);
        jumpingFSM.MakeTransition(State.Jumping, Trigger.Grapple, State.Grappling);
        jumpingFSM.MakeTransition(State.Falling, Trigger.Grapple, State.Grappling);
        jumpingFSM.MakeTransition(State.Grappling, Trigger.Fall, State.Falling);

        jumpingFSM.StartBy(State.Falling);

        actionIdleState.InitializeState(actionFSM, State.Idle, this, orders, blackboard);
        pushingState.InitializeState(actionFSM, State.Pushing, this, orders, blackboard);
        hiddenState.InitializeState(actionFSM, State.Hidden, this, orders, blackboard);

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

    public override bool ShouldBeSaved()
    {
        return true;
    }

    protected override void OnSave(SaveData data)
    {
        data.AddValue("x", transform.position.x);
        data.AddValue("y", transform.position.y);
        

        if(CurrentCollectableObject != null)
        {
            data.AddValue("collectableGUID", CurrentCollectableObject.saveGUID);
        }
        
    }

    protected override void OnLoad(SaveData data)
    {
        /*transform.position = new Vector2(data.GetAs<float>("x"), data.GetAs<float>("y"));

        if(data.ContainsValue("collectableGUID"))
        {
            Guid objGuid = new Guid(data.GetAs<string>("collectableGUID"));

            CurrentCollectableObject = GetSJMonobehaviourSaveableBySaveGUID<CollectableObject>(objGuid);

            CurrentCollectableObject.Collect(this);
        }*/
    }

    public override void PostLoadCallback(SaveData data)
    {
        transform.position = new Vector2(data.GetAs<float>("x"), data.GetAs<float>("y"));

        if(data.ContainsValue("collectableGUID"))
        {
            Guid objGuid = new Guid(data.GetAs<string>("collectableGUID"));

            CurrentCollectableObject = GetSJMonobehaviourSaveableBySaveGUID<CollectableObject>(objGuid);

            CurrentCollectableObject.Collect(this);
        }
    }
}
