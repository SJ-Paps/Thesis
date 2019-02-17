
/*public class TurretChargeSubstate : TurretAttackSubstate
{
    private SyncTimer chargeTimer;

    [SerializeField]
    private float chargeTime;

    private float previousVelocity;

    public override void InitializeState(FSM<TurretAttackState.State, TurretAttackState.Trigger> stateMachine, TurretAttackState.State state, Character character, Character.Blackboard blackboard)
    {
        base.InitializeState(stateMachine, state, character, blackboard);

        chargeTimer = new SyncTimer();
        chargeTimer.Interval = chargeTime;
        chargeTimer.onTick += Attack;
    }

    protected override void OnEnter()
    {
        previousVelocity = character.MovementVelocity;
        character.MovementVelocity = 0;
        chargeTimer.Start();
    }

    protected override void OnUpdate()
    {
        chargeTimer.Update(Time.deltaTime);
    }

    protected override void OnExit()
    {
        character.MovementVelocity = previousVelocity;
    }

    private void Attack(SyncTimer timer)
    {
        stateMachine.Trigger(TurretAttackState.Trigger.GoNext);
    }
    
}*/
