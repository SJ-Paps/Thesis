using System.Collections;
using System.Collections.Generic;
using SAM.FSM;
using UnityEngine;
using System;

[Serializable]
public class TurretAttackState : CharacterState
{

    private DeadlyType deadly = DeadlyType.Bullet;

    private int targetLayers = 1 << Reg.playerLayer;

    [SerializeField]
    private Transform gunPoint;

    public TurretAttackState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard) : base(fsm, state, character, orders, blackboard)
    {
        
    }

    protected override void OnEnter()
    {
        Shoot();
        stateMachine.Trigger(Character.Trigger.StopAttacking);
    }

    protected override void OnExit()
    {
        
    }

    protected override void OnUpdate()
    {
        
    }

    private void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(gunPoint.position, character.transform.up, 4, targetLayers);

        if(hit.collider != null)
        {
            IMortal mortal = hit.collider.GetComponent<IMortal>();

            if(mortal != null)
            {
                mortal.Die(deadly);
            }
        }
    }

    public override void OnAfterDeserialize()
    {
        deadly = DeadlyType.Bullet;
        targetLayers = 1 << Reg.playerLayer;
    }
}
