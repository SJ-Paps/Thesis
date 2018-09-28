using SAM.FSM;
using System.Collections.Generic;

public class XenophobicAttackState : CharacterAttackState
{
    new private Xenophobic character;
    private Weapon weapon;

    public XenophobicAttackState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Xenophobic controller, List<Character.Order> orderList, Character.Blackboard blackboard) : base(fsm, state, controller, orderList, blackboard)
    {
        character = controller;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        EditorDebug.Log("XENOPHOBIC ATTACK ENTER");

        weapon = character.Weapon;
        weapon.UseWeapon();
        character.Animator.SetTrigger("Attack");
    }

    protected override void OnUpdate()
    {
        if (weapon.BeingUsed == false)
        {
            stateMachine.Trigger(Character.Trigger.StopAttacking);
        }
    }

    protected override void OnExit()
    {
        base.OnExit();

        EditorDebug.Log("XENOPHOBIC ATTACK EXIT");
    }
}