using SAM.FSM;
using SAM.Timers;
using System;
using UnityEngine;

[Serializable]
public class XenophobicSeekState : XenophobicIAState
{
    [SerializeField]
    private float positionReachedMarginX = 1f, positionReachedMarginY = 1f;

    private SyncTimer renewPatrolTimer;

    [SerializeField]
    private float renewPatrolTime = 4f;

    private bool hasPositionTarget;
    private Vector2 lastSeekedPosition;

    private Action<Vector2> updatePositionDelegate;

    [SerializeField]
    private float movementVelocity = 4.5f;

    private float previousVelocity;
    
    private int targetLayers;

    [SerializeField]
    private float attackDetectionDistance = 1f;

    public override void InitializeState(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, controller, blackboard);

        updatePositionDelegate += UpdatePosition;

        renewPatrolTimer = new SyncTimer();
        renewPatrolTimer.Interval = renewPatrolTime;
        renewPatrolTimer.onTick += RenewPatrol;

        targetLayers = (1 << Reg.playerLayer);
    }

    protected override void OnEnter()
    {
        previousVelocity = controller.Slave.MovementVelocity;
        controller.Slave.MovementVelocity = movementVelocity;

        blackboard.onLastDetectionPositionChanged += updatePositionDelegate;
    }

    protected override void OnExit()
    {
        controller.Slave.MovementVelocity = previousVelocity;

        blackboard.onLastDetectionPositionChanged -= updatePositionDelegate;
    }

    protected override void OnUpdate()
    {
        renewPatrolTimer.Update(Time.deltaTime);

        if (ShouldStop() == false && hasPositionTarget && IsPositionReached(lastSeekedPosition) == false)
        {
            SearchAtPosition(lastSeekedPosition);
        }
        else
        {
            if (blackboard.PlayerData != null && controller.Slave.Eyes.IsVisibleAndNear(GameManager.Instance.GetPlayer().Collider, Reg.walkableLayerMask, targetLayers, attackDetectionDistance))
            {
                controller.Slave.SetOrder(Character.Order.OrderAttack);
            }

            if (renewPatrolTimer.Active == false)
            {
                renewPatrolTimer.Start();
                hasPositionTarget = false;
            }
        }
    }

    private void RenewPatrol(SyncTimer timer)
    {
        stateMachine.Trigger(XenophobicIAController.Trigger.Patrol);
    }

    private void UpdatePosition(Vector2 lastPosition)
    {
        lastSeekedPosition = lastPosition;
        hasPositionTarget = true;

        if(renewPatrolTimer.Active)
        {
            renewPatrolTimer.Stop();
        }
    }

    private void SearchAtPosition(Vector2 position)
    {
        if (position.x < controller.Slave.transform.position.x)
        {
            controller.Slave.SetOrder(Character.Order.OrderMoveLeft);
        }
        else
        {
            controller.Slave.SetOrder(Character.Order.OrderMoveRight);
        }
    }

    private bool IsPositionReached(Vector2 position)
    {
        Bounds b = new Bounds(position, new Vector2(positionReachedMarginX * 2, positionReachedMarginY * 2));

        if (b.Contains(controller.Slave.transform.position))
        {
            return true;
        }

        return false;
    }

    private bool ShouldStop()
    {
        return controller.Slave.CheckWall(Reg.walkableLayerMask) || controller.Slave.CheckFloorAhead(Reg.walkableLayerMask) == false;
    }

}
