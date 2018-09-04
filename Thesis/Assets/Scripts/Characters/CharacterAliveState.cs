using SAM.FSM;
using System.Collections.Generic;

public class CharacterAliveState : CharacterState
{
    private List<FSM<Character.State, Character.Trigger>> stateMachines;

    private int currentIndex;

    public CharacterAliveState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character controller, List<Character.Order> orderList) : base(fsm, state, controller, orderList)
    {
        stateMachines = new List<FSM<Character.State, Character.Trigger>>();
    }

    protected override void OnUpdate()
    {
        for(currentIndex = 0; currentIndex < stateMachines.Count; currentIndex++)
        {
            stateMachines[currentIndex].UpdateCurrentState();
        }
    }

    public void AddFSM(FSM<Character.State, Character.Trigger> fsm)
    {
        stateMachines.Add(fsm);
    }

    public void RemoveFSM(FSM<Character.State, Character.Trigger> fsm)
    {
        if(stateMachines.Remove(fsm))
        {
            currentIndex--;
        }
    }
}
