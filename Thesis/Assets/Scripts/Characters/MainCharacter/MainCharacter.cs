using SAM.FSM;

public class MainCharacter : Character
{
    public override event ChangeControlDelegate onChangeControl;

    private FSM<State, Trigger>[] stateMachines;

    void Awake()
    {
        FSM<State, Trigger> movementFSM = new FSM<State, Trigger>();

        movementFSM.AddState(new IdleState(movementFSM, State.Idle, this));
        movementFSM.AddState(new MovingState(movementFSM, State.Moving, this));

        movementFSM.MakeTransition(State.Idle, Trigger.Move, State.Moving);
        movementFSM.MakeTransition(State.Moving, Trigger.StopMoving, State.Idle);

        movementFSM.StartBy(State.Idle);

        FSM<State, Trigger> jumpingFSM = new FSM<State, Trigger>();

        jumpingFSM.AddState(new GroundedState(jumpingFSM, State.Grounded, this));
        jumpingFSM.AddState(new JumpingState(jumpingFSM, State.Jumping, this));
        jumpingFSM.AddState(new FallingState(jumpingFSM, State.Falling, this));

        jumpingFSM.MakeTransition(State.Grounded, Trigger.Jump, State.Jumping);
        jumpingFSM.MakeTransition(State.Jumping, Trigger.Fall, State.Falling);
        jumpingFSM.MakeTransition(State.Falling, Trigger.Ground, State.Grounded);

        jumpingFSM.StartBy(State.Falling);

        stateMachines = new FSM<State, Trigger>[2];

        stateMachines[0] = movementFSM;
        stateMachines[1] = jumpingFSM;
    }

    void Update()
    {
        for(int i = 0; i < stateMachines.Length; i++)
        {
            stateMachines[i].UpdateCurrentState();
        }
    }

    public override void GetEnslaved()
    {
        enslaved = true;
    }

    public override void SetOrder(Order e)
    {
        for(int i = 0; i < stateMachines.Length; i++)
        {
            ((CharacterState)stateMachines[i].CurrentState).ProcessOrder(e);
        }
    }
}
