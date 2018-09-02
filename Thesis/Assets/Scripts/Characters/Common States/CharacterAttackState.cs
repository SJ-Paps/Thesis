using SAM.FSM;

public abstract class CharacterAttackState : CharacterState
{
    public CharacterAttackState(FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs> fsm, Character.State state, Character controller) : base(fsm, state, controller)
    {

    }
}
