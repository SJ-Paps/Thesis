using SAM.FSM;
using UnityEngine;

public class ActionsIdleState : CharacterState 
{
    private HiddenState hiddenState;
    public float actualCooldownToHide;
    public float necessaryCooldownToHide;

    public ActionsIdleState(FSM<Character.State, Character.Trigger, Character.ChangedStateEventArgs> fsm, Character.State state, Character character) : base(fsm, state, character)
    {
        hiddenState = character.GetComponent<HiddenState>();
        actualCooldownToHide = 2.0f;
        necessaryCooldownToHide = 2.0f;
    }

    protected override void OnUpdate()
    {
        actualCooldownToHide += Time.time;

        while (eventQueue.Count != 0) {
            Character.Order ev = eventQueue.Dequeue();

            if (ev == Character.Order.OrderHide && character.isHiding == true && actualCooldownToHide >= necessaryCooldownToHide) {
                EditorDebug.Log(character.isHiding);
                character.GetComponent<Collider2D>().isTrigger = true;
                character.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                stateMachine.Trigger(Character.Trigger.Hide);
            }
        }
    }
}
