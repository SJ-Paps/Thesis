using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SAM.FSM;

[Serializable]
public class TurretWithoutTargetState : TurretIAState
{
    private int blockingLayers;
    private int targetLayers;
    private Eyes turretEyes;

    private Action<Collider2D> analyzeDetectionDelegate;

    private Character.Order currentMoveOrder;

    protected override void OnEnter()
    {
        turretEyes = controller.CharacterEyes;

        if(turretEyes != null)
        {
            turretEyes.onStay += analyzeDetectionDelegate;
        }
    }

    protected override void OnUpdate()
    {
        Scan();
    }

    protected override void OnExit()
    {
        if(turretEyes != null)
        {
            turretEyes.onStay -= analyzeDetectionDelegate;
        }
    }

    public override void InitializeState(FSM<TurretIAController.State, TurretIAController.Trigger> stateMachine, TurretIAController.State state, TurretIAController controller, TurretIAController.Blackboard blackboard)
    {
        base.InitializeState(stateMachine, state, controller, blackboard);

        analyzeDetectionDelegate = AnalyzeDetection;
        blockingLayers = (1 << Reg.wallLayer) | (1 << Reg.floorLayer) | (1 << Reg.objectLayer);
        targetLayers = 1 << Reg.playerLayer;
        currentMoveOrder = Character.Order.OrderMoveLeft;
    }

    private void AnalyzeDetection(Collider2D collider)
    {
        Debug.Log("ANALYZING");

        if(collider.gameObject.layer == Reg.playerLayer && turretEyes.IsVisible(collider, blockingLayers, targetLayers))
        {
            Debug.Log("ENTRO");

            blackboard.TargetPosition = collider.transform.position;
            stateMachine.Trigger(TurretIAController.Trigger.TargetFound);
        }
    }

    private void Scan()
    {
        if(controller.Slave.IsOverLimit())
        {
            if(currentMoveOrder == Character.Order.OrderMoveLeft)
            {
                currentMoveOrder = Character.Order.OrderMoveRight;
            }
            else
            {
                currentMoveOrder = Character.Order.OrderMoveLeft;
            }
        }

        controller.Slave.SetOrder(currentMoveOrder);
    }
}
