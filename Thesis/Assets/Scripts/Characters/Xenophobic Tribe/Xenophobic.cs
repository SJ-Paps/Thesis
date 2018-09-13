using SAM.FSM;
using UnityEngine;
using System;

public class Xenophobic : Tribal, IAudibleListener
{
    [SerializeField]
    protected Weapon weapon;

    public Weapon Weapon
    {
        get
        {
            return weapon;
        }

        protected set
        {
            weapon = value;
        }
    }

    protected FSM<State, Trigger> attackFSM;
    protected FSM<State, Trigger> movementFSM;
    protected FSM<State, Trigger> jumpingFSM;

    protected override void Awake()
    {
        base.Awake();

        movementFSM = new FSM<State, Trigger>();

        movementFSM.AddState(new CharacterIdleState(movementFSM, State.Idle, this, orders, blackboard));
        movementFSM.AddState(new MovingState(movementFSM, State.Moving, this, orders, blackboard));

        movementFSM.MakeTransition(State.Idle, Trigger.Move, State.Moving);
        movementFSM.MakeTransition(State.Moving, Trigger.StopMoving, State.Idle);

        movementFSM.StartBy(State.Idle);
        

        jumpingFSM = new FSM<State, Trigger>();

        jumpingFSM.AddState(new GroundedState(jumpingFSM, State.Grounded, this, orders, blackboard));
        jumpingFSM.AddState(new JumpingState(jumpingFSM, State.Jumping, this, orders, blackboard));
        jumpingFSM.AddState(new FallingState(jumpingFSM, State.Falling, this, orders, blackboard));

        jumpingFSM.MakeTransition(State.Grounded, Trigger.Jump, State.Jumping);
        jumpingFSM.MakeTransition(State.Grounded, Trigger.Fall, State.Falling);
        jumpingFSM.MakeTransition(State.Jumping, Trigger.Fall, State.Falling);
        jumpingFSM.MakeTransition(State.Falling, Trigger.Ground, State.Grounded);

        jumpingFSM.StartBy(State.Falling);


        attackFSM = new FSM<State, Trigger>();

        attackFSM.AddState(State.Idle);
        attackFSM.AddState(new XenophobicAttackState(attackFSM, State.Attacking, this, orders, blackboard));

        attackFSM.MakeTransition(State.Idle, Trigger.Attack, State.Attacking);
        attackFSM.MakeTransition(State.Attacking, Trigger.StopAttacking, State.Idle);

        attackFSM.StartBy(State.Idle);

        AddStateMachineWhenAlive(movementFSM);
        AddStateMachineWhenAlive(jumpingFSM);
        AddStateMachineWhenAlive(attackFSM);
    }

    public override void GetEnslaved()
    {

    }

    public void Listen(ref AudibleData data)
    {
        
    }
}
