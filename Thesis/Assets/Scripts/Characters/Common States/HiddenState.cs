using SAM.FSM;
using UnityEngine;

public class HiddenState : CharacterState 
{
    public HiddenState(FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs> fsm, Character.State state, Character character) : base(fsm, state, character)
    {

    }

    protected override void OnEnter(ref Character.ChangedStateEventArgs e) 
    {
        base.OnEnter(ref e);

        EditorDebug.Log("Entrado a Hide");
    }

    protected override void OnExit()
    {
        base.OnExit();

        EditorDebug.Log("Salí de Hide");
    }

    protected override void OnUpdate() 
    {
        Hide();
    }

    private void Hide() 
    {
        while (eventQueue.Count != 0)
        {
            Character.Order ev = eventQueue.Dequeue();

            if (ev == Character.Order.OrderHide) 
            {
                stateMachine.Trigger(Character.Trigger.GoIdle);
            }
        }
    }
}
