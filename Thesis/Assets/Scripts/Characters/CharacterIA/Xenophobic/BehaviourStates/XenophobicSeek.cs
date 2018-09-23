using SAM.FSM;
using SAM.Timers;
using System;
using UnityEngine;

public class XenophobicSeek : XenophobicIAState
{
    private float positionReachedMarginX = 1f;
    private float positionReachedMarginY = 1f;

    private SyncTimer renewPatrolTimer;
    private float renewPatrolTime = 4f;

    private bool hasPositionTarget;
    private Vector2 lastSeekedPosition;

    private Action<Vector2> updatePositionDelegate;

    public XenophobicSeek(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state, controller, blackboard)
    {
        updatePositionDelegate += UpdatePosition;

        renewPatrolTimer = new SyncTimer();
        renewPatrolTimer.Interval = renewPatrolTime;
        renewPatrolTimer.onTick += RenewPatrol;
        
    }

    protected override void OnEnter()
    {
        blackboard.onLastDetectionPositionChanged += updatePositionDelegate;
    }

    protected override void OnExit()
    {
        blackboard.onLastDetectionPositionChanged -= updatePositionDelegate;
    }

    protected override void OnUpdate()
    {
        renewPatrolTimer.Update(Time.deltaTime);

        if(hasPositionTarget && IsPositionReached(lastSeekedPosition) == false)
        {
            SearchAtPosition(lastSeekedPosition);
        }
        else if(renewPatrolTimer.Active == false)
        {
            renewPatrolTimer.Start();
            hasPositionTarget = false;
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

}
