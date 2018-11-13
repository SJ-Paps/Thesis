using SAM.FSM;
using System.Collections.Generic;
using System;

[Serializable]
public class XenophobicAttackState : CharacterState
{
    private Xenophobic xenophobic;
    private Weapon weapon;

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);

        xenophobic = (Xenophobic)character;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        EditorDebug.Log("XENOPHOBIC ATTACK ENTER");

        weapon = xenophobic.Weapon;
        weapon.UseWeapon();
        xenophobic.Animator.SetTrigger("Attack");
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