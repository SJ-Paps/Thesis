public class TurretAttackState : CharacterHSMState
{

    public TurretAttackState(Character.State state, string debugName) : base(state, debugName)
    {
        /*chargeState.InitializeState(attackSubStateMachine, State.Charging, character, blackboard);
        shootState.InitializeState(attackSubStateMachine, State.Shooting, character, blackboard);

        attackSubStateMachine.AddState(State.Idle);
        attackSubStateMachine.AddState(chargeState);
        attackSubStateMachine.AddState(shootState);

        attackSubStateMachine.MakeTransition(State.Idle, Trigger.GoNext, State.Charging);
        attackSubStateMachine.MakeTransition(State.Charging, Trigger.GoNext, State.Shooting);
        attackSubStateMachine.MakeTransition(State.Shooting, Trigger.GoNext, State.Idle);

        attackSubStateMachine.StartBy(State.Idle);

        attackSubStateMachine.onStateChanged += OnStateChanged;*/
    }

    /*private void OnStateChanged(State previous, State current)
    {
        if (previous == State.Shooting)
        {
            stateMachine.Trigger(Character.Trigger.StopAttacking);
        }
    }

    protected override void OnEnter()
    {
        attackSubStateMachine.Trigger(Trigger.GoNext);
    }

    protected override void OnExit()
    {
        
    }

    protected override void OnUpdate()
    {
        attackSubStateMachine.UpdateCurrentState();
    }*/

    
}

/*public class TurretAttackSubstate : State<TurretAttackState.State, TurretAttackState.Trigger>
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
}*/
