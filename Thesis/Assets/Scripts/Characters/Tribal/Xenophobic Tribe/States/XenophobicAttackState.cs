using SAM.FSM;
using System.Collections.Generic;
using System;

public class XenophobicAttackState : CharacterHSMState
{
    private Xenophobic xenophobic;
    private Weapon weapon;

    public XenophobicAttackState(Character.State state, string debugName) : base(state, debugName)
    {
        xenophobic = (Xenophobic)character;
    }

    /*protected override void OnEnter()
    {
        base.OnEnter();

        EditorDebug.Log("XENOPHOBIC ATTACK ENTER");

        if (xenophobic.CurrentCollectableObject != null && xenophobic.CurrentCollectableObject is Weapon)
        {
            weapon = (Weapon)xenophobic.CurrentCollectableObject;
            weapon.Activate(xenophobic);
            xenophobic.Animator.SetTrigger("Attack");
        }
        else
        {
            stateMachine.Trigger(Character.Trigger.StopAttacking);
        }
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
    }*/
}