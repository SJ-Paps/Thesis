using SAM.FSM;
using System.Collections.Generic;
using System;

[Serializable]
public class CharacterDeadState : CharacterState
{
    private List<FSM<Character.State, Character.Trigger>> stateMachines;

    private int currentIndex;

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);

        stateMachines = new List<FSM<Character.State, Character.Trigger>>();
    }

    protected override void OnUpdate()
    {
        for (currentIndex = 0; currentIndex < stateMachines.Count; currentIndex++)
        {
            stateMachines[currentIndex].UpdateCurrentState();
        }

        base.OnUpdate();
    }

    public void AddFSM(FSM<Character.State, Character.Trigger> fsm)
    {
        stateMachines.Add(fsm);
    }

    public void RemoveFSM(FSM<Character.State, Character.Trigger> fsm)
    {
        if (stateMachines.Remove(fsm))
        {
            currentIndex--;
        }
    }
}
