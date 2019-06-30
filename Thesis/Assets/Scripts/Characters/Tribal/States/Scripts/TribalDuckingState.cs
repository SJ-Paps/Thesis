using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalDuckingState : TribalHSMState
{
    private Vector2 previousOffset;
    private Vector2 previousSize;

    private float velocityConstraintPercentage = 60;

    private int velocityContraintId;

    private bool shouldStandUp;

    protected override void OnEnter()
    {
        base.OnEnter();

        SJCapsuleCollider2D Collider = (SJCapsuleCollider2D)Configuration.Collider;

        float yFloor = Collider.bounds.min.y;

        previousOffset = Collider.offset;
        previousSize = Collider.InnerCollider.size;
        
        float colliderSizeY = previousSize.y / 2;

        if(colliderSizeY < previousSize.x)
        {
            colliderSizeY = previousSize.x;
        }

        Collider.InnerCollider.size = new Vector2(previousSize.x, colliderSizeY);

        float distanceFromZeroOffsetToFloor = Mathf.Abs((Owner.transform.position.y) - yFloor);

        Collider.offset = new Vector2(previousOffset.x, (previousOffset.y - distanceFromZeroOffsetToFloor / 2));

        velocityContraintId = Owner.MaxVelocity.AddPercentageConstraint(velocityConstraintPercentage);

        shouldStandUp = false;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (shouldStandUp)
        {
            SendEvent(Character.Order.StandUp);
        }

        shouldStandUp = true;
    }

    protected override void OnExit()
    {
        base.OnExit();

        SJCapsuleCollider2D Collider = (SJCapsuleCollider2D)Configuration.Collider;

        Collider.InnerCollider.size = previousSize;
        Collider.offset = previousOffset;

        Owner.MaxVelocity.RemovePercentageConstraint(velocityContraintId);
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if(trigger == Character.Order.Duck)
        {
            shouldStandUp = false;
            return true;
        }

        return false;
    }
}