using System.Collections;
using System.Collections.Generic;
using SAM.FSM;
using UnityEngine;
using System;

[Serializable]
public class TurretAttackState : CharacterState
{
    public enum State
    {
        Charging,
        Shooting
    }

    public enum Trigger
    {
        GoNext
    }

    [SerializeField]
    private TurretChargeSubstate chargeState;

    [SerializeField]
    private TurretShootSubstate shootState;

    private FSM<State, Trigger> attackSubStateMachine;

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);

        attackSubStateMachine = new FSM<State, Trigger>();

        chargeState.InitializeState(attackSubStateMachine, State.Charging, character, blackboard);
        shootState.InitializeState(attackSubStateMachine, State.Shooting, character, blackboard);

        attackSubStateMachine.AddState(chargeState);
        attackSubStateMachine.AddState(shootState);

        attackSubStateMachine.MakeTransition(State.Charging, Trigger.GoNext, State.Shooting);
        attackSubStateMachine.MakeTransition(State.Shooting, Trigger.GoNext, State.Charging);

        attackSubStateMachine.StartBy(State.Charging);

        attackSubStateMachine.onStateChanged += OnStateChanged;
    }

    private void OnStateChanged(State previous, State current)
    {
        if (previous == State.Shooting)
        {
            stateMachine.Trigger(Character.Trigger.StopAttacking);
        }
    }

    protected override void OnEnter()
    {

    }

    protected override void OnExit()
    {
        
    }

    protected override void OnUpdate()
    {
        attackSubStateMachine.UpdateCurrentState();
    }

    
}

public class TurretAttackSubstate : State<TurretAttackState.State, TurretAttackState.Trigger>
{
    protected Character.Blackboard blackboard;
    protected Character character;

    public TurretAttackSubstate() : base(null, default(TurretAttackState.State))
    {

    }

    public virtual void InitializeState(FSM<TurretAttackState.State, TurretAttackState.Trigger> stateMachine, TurretAttackState.State state, Character character, Character.Blackboard blackboard)
    {
        this.stateMachine = stateMachine;
        this.InnerState = state;
        this.character = character;
        this.blackboard = blackboard;
    }

    protected override void OnEnter()
    {
        
    }

    protected override void OnUpdate()
    {
        
    }

    protected override void OnExit()
    {
        
    }
}
