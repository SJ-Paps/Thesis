﻿using UnityEngine;

public class TribalFallingState : TribalHSMState
{
    private float velocityDeadZone = -0.002f;

    protected override void OnEnter()
    {
        base.OnEnter();

        Configuration.Animator.SetTrigger(Tribal.FallAnimatorTrigger);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(Configuration.RigidBody2D.velocity.y > velocityDeadZone)
        {
            if (IsOnFloor(Reg.walkableLayerMask))
            {
                SendEvent(Character.Order.Ground);
            }
            
        }
    }

    protected override void OnExit()
    {
        base.OnExit();

        Configuration.Animator.ResetTrigger(Tribal.FallAnimatorTrigger);
    }


    private bool IsOnFloor(int layerMask)
    {
        Bounds bounds = Configuration.Collider.bounds;
        float height = 0.05f;
        float checkFloorNegativeOffsetX = -0.1f;

        Vector2 leftPoint = new Vector2(bounds.center.x - bounds.extents.x - checkFloorNegativeOffsetX, bounds.center.y - bounds.extents.y);
        Vector2 rightPoint = new Vector2(bounds.center.x + bounds.extents.x + checkFloorNegativeOffsetX, bounds.center.y - bounds.extents.y);

        EditorDebug.DrawLine(leftPoint, new Vector3(rightPoint.x, rightPoint.y - height), Color.green);
        EditorDebug.DrawLine(rightPoint, new Vector3(leftPoint.x, leftPoint.y - height), Color.green);

        return Physics2D.Linecast(leftPoint, new Vector2(rightPoint.x, rightPoint.y - height), layerMask) ||
            Physics2D.Linecast(rightPoint, new Vector2(leftPoint.x, leftPoint.y - height), layerMask);

    }

}
