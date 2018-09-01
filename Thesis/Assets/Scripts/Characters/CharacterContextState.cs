using System.Collections.Generic;
using SAM.FSM;

public class CharacterContextState : CharacterState
{
    protected List<FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs>> stateMachines;

    public CharacterContextState(FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs> fsm, Character.State state, Character controller) : base(fsm, state, controller)
    {
        stateMachines = new List<FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs>>();
    }

    protected override void OnEnter(ref Character.ChangedStateEventArgs e)
    {
        
    }

    protected override void OnExit()
    {
        
    }

    protected override void OnUpdate()
    {
        for(int i = 0; i < stateMachines.Count; i++)
        {
            stateMachines[i].UpdateCurrentState();
        }
    }

    public void AddFSM(FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs> fsm)
    {
        stateMachines.Add(fsm);
    }
}
