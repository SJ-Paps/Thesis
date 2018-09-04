using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;

public class HiddenState : CharacterState 
{
    private ActionsIdleState actionsIdleState;

    public HiddenState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders) : base(fsm, state, character, orders)
    {

    }

    protected override void OnEnter() 
    {
        actionsIdleState.actualCooldownToHide = 0;
        EditorDebug.Log("Entrado a Hide");
    }

    protected override void OnExit()
    {
        EditorDebug.Log("Salí de Hide");
    }

    protected override void OnUpdate() 
    {
        Hide();
    }

    private void Hide() 
    {
        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order ev = orders[i];

            if (ev == Character.Order.OrderHide && character.isHiding == true) 
            {
                character.isHiding = false;
                character.GetComponent<Collider2D>().isTrigger = false;
                character.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                stateMachine.Trigger(Character.Trigger.StopHiding);
            }
        }
    }
}
