using SAM.FSM;
using System;

public class MainCharacter : Tribal
{
    protected override void Awake()
    {
        base.Awake();

        FSM<State, Trigger, ChangedStateEventArgs> movementFSM = new FSM<State, Trigger, ChangedStateEventArgs>();

        movementFSM.AddState(new CharacterIdleState(movementFSM, State.Idle, this));
        movementFSM.AddState(new MovingState(movementFSM, State.Moving, this));

        movementFSM.MakeTransition(State.Idle, Trigger.Move, State.Moving);
        movementFSM.MakeTransition(State.Moving, Trigger.StopMoving, State.Idle);

        movementFSM.StartBy(State.Idle);

        FSM<State, Trigger, ChangedStateEventArgs> jumpingFSM = new FSM<State, Trigger, ChangedStateEventArgs>();

        jumpingFSM.AddState(new GroundedState(jumpingFSM, State.Grounded, this));
        jumpingFSM.AddState(new JumpingState(jumpingFSM, State.Jumping, this));
        jumpingFSM.AddState(new FallingState(jumpingFSM, State.Falling, this));

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

    protected override void Attack()
    {
        
    }
}
