using SAM.FSM;

public class MainCharacter : Tribal
{
    protected override void Awake()
    {
        base.Awake();

        FSM<State, Trigger> movementFSM = new FSM<State, Trigger>();

        movementFSM.AddState(new CharacterIdleState(movementFSM, State.Idle, this, orders));
        movementFSM.AddState(new MovingState(movementFSM, State.Moving, this, orders));

        movementFSM.MakeTransition(State.Idle, Trigger.Move, State.Moving);
        movementFSM.MakeTransition(State.Moving, Trigger.StopMoving, State.Idle);

        movementFSM.StartBy(State.Idle);

        FSM<State, Trigger> jumpingFSM = new FSM<State, Trigger>();

        jumpingFSM.AddState(new GroundedState(jumpingFSM, State.Grounded, this, orders));
        jumpingFSM.AddState(new JumpingState(jumpingFSM, State.Jumping, this, orders));
        jumpingFSM.AddState(new FallingState(jumpingFSM, State.Falling, this, orders));

        jumpingFSM.MakeTransition(State.Grounded, Trigger.Jump, State.Jumping);
        jumpingFSM.MakeTransition(State.Jumping, Trigger.Fall, State.Falling);
        jumpingFSM.MakeTransition(State.Falling, Trigger.Ground, State.Grounded);

        jumpingFSM.StartBy(State.Falling);

        FSM<State, Trigger> actionsFSM = new FSM<State, Trigger>();

        actionsFSM.AddState(new HiddenState(actionsFSM, State.Hidden, this, orders));
        actionsFSM.AddState(new ActionsIdleState(actionsFSM, State.Idle, this, orders));

        actionsFSM.MakeTransition(State.Idle, Trigger.Hide, State.Hidden);
        actionsFSM.MakeTransition(State.Hidden, Trigger.StopHiding, State.Idle);

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
