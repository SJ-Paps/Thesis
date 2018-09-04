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

        AddStateMachineWhenAlive(movementFSM);
        AddStateMachineWhenAlive(jumpingFSM);

        gameObject.layer = Reg.playerLayer;
    }

    public override void GetEnslaved()
    {
        enslaved = true;
    }
}
