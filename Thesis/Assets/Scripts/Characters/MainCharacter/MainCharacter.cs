using SAM.FSM;

public class MainCharacter : Tribal
{
    protected override void Awake()
    {
        base.Awake();

        FSM<State, Trigger> movementFSM = new FSM<State, Trigger>();

        movementFSM.AddState(new CharacterIdleState(movementFSM, State.Idle, this, orders, blackboard));
        movementFSM.AddState(new MovingState(movementFSM, State.Moving, this, orders, blackboard));

        movementFSM.MakeTransition(State.Idle, Trigger.Move, State.Moving);
        movementFSM.MakeTransition(State.Moving, Trigger.StopMoving, State.Idle);

        movementFSM.StartBy(State.Idle);

        FSM<State, Trigger> jumpingFSM = new FSM<State, Trigger>();

        jumpingFSM.AddState(new GroundedState(jumpingFSM, State.Grounded, this, orders, blackboard));
        jumpingFSM.AddState(new JumpingState(jumpingFSM, State.Jumping, this, orders, blackboard));
        jumpingFSM.AddState(new FallingState(jumpingFSM, State.Falling, this, orders, blackboard));

        jumpingFSM.MakeTransition(State.Grounded, Trigger.Jump, State.Jumping);
        jumpingFSM.MakeTransition(State.Grounded, Trigger.Fall, State.Falling);
        jumpingFSM.MakeTransition(State.Jumping, Trigger.Fall, State.Falling);
        jumpingFSM.MakeTransition(State.Falling, Trigger.Ground, State.Grounded);

        jumpingFSM.StartBy(State.Falling);

        FSM<State, Trigger> actionsFSM = new FSM<State, Trigger>();

        actionsFSM.AddState(new HiddenState(actionsFSM, State.Hidden, this, orders, blackboard, jumpingFSM, movementFSM));
        actionsFSM.AddState(new PushingObjectState(actionsFSM, State.Pushing, this, orders, blackboard, jumpingFSM, movementFSM));
        actionsFSM.AddState(new ActionsIdleState(actionsFSM, State.Idle, this, orders, jumpingFSM, movementFSM, blackboard));

        actionsFSM.MakeTransition(State.Idle, Trigger.Hide, State.Hidden);
        actionsFSM.MakeTransition(State.Hidden, Trigger.StopHiding, State.Idle);

        actionsFSM.MakeTransition(State.Idle, Trigger.Push, State.Pushing);
        actionsFSM.MakeTransition(State.Pushing, Trigger.StopPushing, State.Idle);

        actionsFSM.StartBy(State.Idle);

        AddStateMachineWhenAlive(movementFSM);
        AddStateMachineWhenAlive(jumpingFSM);
        AddStateMachineWhenAlive(actionsFSM);

        gameObject.layer = Reg.playerLayer;
    }

    public override void GetEnslaved()
    {
        enslaved = true;
    }
}
