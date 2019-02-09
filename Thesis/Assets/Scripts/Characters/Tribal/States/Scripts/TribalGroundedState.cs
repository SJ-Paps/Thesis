using SAM.Timers;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TribalGroundedState : TribalHSMState
{
    private float velocityDeadZone = 0.002f;

    public TribalGroundedState(Character.State state, string debugName) : base(state, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        character.Animator.SetTrigger(Tribal.GroundAnimatorTriggerName);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (character.RigidBody2D.velocity.y < velocityDeadZone && IsOnFloor(Reg.walkableLayerMask) == false)
        {
            SendEvent(Character.Trigger.Fall);
        }
    }

    protected override void OnExit()
    {
        base.OnExit();

        character.Animator.ResetTrigger(Tribal.GroundAnimatorTriggerName);
    }

    private bool IsOnFloor(int layerMask)
    {
        Bounds bounds = character.Collider.bounds;
        float height = 0.05f;

        Vector2 leftPoint = new Vector2(bounds.center.x - bounds.extents.x, bounds.center.y - bounds.extents.y);
        Vector2 rightPoint = new Vector2(bounds.center.x + bounds.extents.x, bounds.center.y - bounds.extents.y);

        EditorDebug.DrawLine(leftPoint, new Vector3(rightPoint.x, rightPoint.y - height), Color.green);
        EditorDebug.DrawLine(rightPoint, new Vector3(leftPoint.x, leftPoint.y - height), Color.green);

        return Physics2D.Linecast(leftPoint, new Vector2(rightPoint.x, rightPoint.y - height), layerMask) ||
            Physics2D.Linecast(rightPoint, new Vector2(leftPoint.x, leftPoint.y - height), layerMask);

    }
}
