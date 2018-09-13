using SAM.FSM;
using System.Collections.Generic;

public abstract class CharacterAttackState : CharacterState
{
    public CharacterAttackState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character controller, List<Character.Order> orderList, Character.Blackboard blackboard) : base(fsm, state, controller, orderList, blackboard)
    {

    }
}
