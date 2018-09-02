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

        FSM<State, Trigger, ChangedStateEventArgs> actionsFSM = new FSM<State, Trigger, ChangedStateEventArgs>();

        actionsFSM.AddState(new HiddenState(actionsFSM, State.Hidden, this));
        actionsFSM.AddState(new ActionsIdleState(actionsFSM, State.Idle, this));

        actionsFSM.MakeTransition(State.Idle, Trigger.Hide, State.Hidden);
        actionsFSM.MakeTransition(State.Hidden, Trigger.GoIdle, State.Idle);

        actionsFSM.StartBy(State.Idle);

        AddStateMachineWhenAlive(movementFSM);
        AddStateMachineWhenAlive(jumpingFSM);
        AddStateMachineWhenAlive(actionsFSM);
    }

    public override void GetEnslaved()
    {
        enslaved = true;
    }
}
