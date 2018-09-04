using SAM.FSM;
using UnityEngine;

public class HiddenState : CharacterState 
{
    private ActionsIdleState actionsIdleState;

    public HiddenState(FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs> fsm, Character.State state, Character character) : base(fsm, state, character)
    {
        actionsIdleState = character.GetComponent<ActionsIdleState>();
    }

    protected override void OnEnter(ref Character.ChangedStateEventArgs e) 
    {
        base.OnEnter(ref e);
        actionsIdleState.actualCooldownToHide = 0;
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

            if (ev == Character.Order.OrderHide && character.isHiding == true) 
            {
                character.isHiding = false;
                character.GetComponent<Collider2D>().isTrigger = false;
                character.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                stateMachine.Trigger(Character.Trigger.GoIdle);
            }
        }
    }
}
