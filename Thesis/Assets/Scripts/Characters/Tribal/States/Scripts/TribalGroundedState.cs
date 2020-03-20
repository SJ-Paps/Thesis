using SJ;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TribalGroundedState : TribalHSMState
{
    private float velocityDeadZone = -2f;

    private SyncTimer groundingTimer;

    public TribalGroundedState()
    {
        groundingTimer = new SyncTimer();

        float groundingInterval = 0.2f;

        groundingTimer.Interval = groundingInterval;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        groundingTimer.Start();

        Owner.Animator.SetTrigger(Tribal.GroundAnimatorTrigger);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        groundingTimer.Update(Time.deltaTime);

        if (Owner.RigidBody2D.velocity.y < velocityDeadZone && IsOnFloor(Reg.walkableLayerMask) == false)
        {
            SendEvent(Character.Order.Fall);
        }
    }

    protected override void OnExit()
    {
        base.OnExit();

        groundingTimer.Stop();

        Owner.Animator.ResetTrigger(Tribal.GroundAnimatorTrigger);
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if(trigger == Character.Order.Jump && groundingTimer.Active)
        {
            return true;
        }

        return false;
    }

    private bool IsOnFloor(int layerMask)
    {
        Bounds bounds = Owner.Collider.bounds;
        float height = 0.05f;
        float checkFloorNegativeOffsetX = -0.1f;

        Vector2 leftPoint = new Vector2(bounds.center.x - bounds.extents.x - checkFloorNegativeOffsetX, bounds.center.y - bounds.extents.y);
        Vector2 rightPoint = new Vector2(bounds.center.x + bounds.extents.x + checkFloorNegativeOffsetX, bounds.center.y - bounds.extents.y);

        Logger.DrawLine(leftPoint, new Vector3(rightPoint.x, rightPoint.y - height), Color.green);
        Logger.DrawLine(rightPoint, new Vector3(leftPoint.x, leftPoint.y - height), Color.green);

        return Physics2D.Linecast(leftPoint, new Vector2(rightPoint.x, rightPoint.y - height), layerMask) ||
            Physics2D.Linecast(rightPoint, new Vector2(leftPoint.x, leftPoint.y - height), layerMask);

    }
}
