using SAM.FSM;

public class CharacterAttackState : CharacterState
{
    public CharacterAttackState(FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs> fsm, Character.State state, Character controller) : base(fsm, state, controller)
    {

    }

    protected void Attack()
    {

    }

    protected override void OnUpdate()
    {
        
    }
}
