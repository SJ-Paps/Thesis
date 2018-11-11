using SAM.FSM;
using System;
using UnityEngine;

[Serializable]
public class TurretShootSubstate : TurretAttackSubstate
{
    [SerializeField]
    private float shootDistance;

    private int targetLayers;

    private DeadlyType deadly;

    [SerializeField]
    private Transform gunPoint;

    public override void InitializeState(FSM<TurretAttackState.State, TurretAttackState.Trigger> stateMachine, TurretAttackState.State state, Character character, Character.Blackboard blackboard)
    {
        base.InitializeState(stateMachine, state, character, blackboard);

        deadly = DeadlyType.Bullet;
        targetLayers = 1 << Reg.playerLayer;
    }

    protected override void OnEnter()
    {
        Shoot();
        stateMachine.Trigger(TurretAttackState.Trigger.GoNext);
    }

    private void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(gunPoint.position, character.transform.up, shootDistance, targetLayers);

        if (hit.collider != null)
        {
            IMortal mortal = hit.collider.GetComponent<IMortal>();

            if (mortal != null)
            {
                mortal.Die(deadly);
            }
        }
    }
}
