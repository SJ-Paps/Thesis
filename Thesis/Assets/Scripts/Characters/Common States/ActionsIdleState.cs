using SAM.FSM;
using UnityEngine;
using System.Collections.Generic;

public class ActionsIdleState : CharacterState 
{
    private HiddenState hiddenState;
    public float actualCooldownToHide;
    public float necessaryCooldownToHide;

    public ActionsIdleState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders) : base(fsm, state, character, orders)
    {
        actualCooldownToHide = 2.0f;
        necessaryCooldownToHide = 2.0f;
    }

    protected override void OnUpdate()
    {
        actualCooldownToHide += Time.time;

        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order ev = orders[i];

            if (ev == Character.Order.OrderHide && character.isHiding == true && actualCooldownToHide >= necessaryCooldownToHide) {
                EditorDebug.Log(character.isHiding);
                character.GetComponent<Collider2D>().isTrigger = true;
                character.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                stateMachine.Trigger(Character.Trigger.Hide);
            }
        }
    }
}
