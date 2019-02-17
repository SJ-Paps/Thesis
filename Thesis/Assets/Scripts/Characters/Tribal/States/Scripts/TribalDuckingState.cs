using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalDuckingState : TribalHSMState
{
    private float colliderSizeY;

    private float previousSizeX;
    private float previousSizeY;
    private float previousOffsetX;
    private float previousOffsetY;

    private float velocityConstraintPercentage = 60;

    private int velocityContraintId;

    private bool shouldStandUp;

    public TribalDuckingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        float yFloor = Owner.Collider.bounds.center.y - Owner.Collider.bounds.extents.y;
        
        previousSizeX = Owner.Collider.GetSize().x;
        previousSizeY = Owner.Collider.GetSize().y;

        previousOffsetX = Owner.Collider.offset.x;
        previousOffsetY = Owner.Collider.offset.y;
        
        colliderSizeY = previousSizeY / 2;

        if(colliderSizeY < previousSizeX)
        {
            colliderSizeY = previousSizeX;
        }
        
        Owner.Collider.ChangeSize(new Vector2(previousSizeX, colliderSizeY));
        Owner.Collider.offset = new Vector2(previousOffsetX, yFloor + colliderSizeY);

        velocityContraintId = Owner.MaxVelocity.AddPercentageConstraint(velocityConstraintPercentage);

        shouldStandUp = false;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (shouldStandUp)
        {
            SendEvent(Character.Trigger.StandUp);
        }

        shouldStandUp = true;
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.Collider.ChangeSize(new Vector2(previousSizeX, previousSizeY));
        Owner.Collider.offset = new Vector2(previousOffsetX, previousOffsetY);

        Owner.MaxVelocity.RemovePercentageConstraint(velocityContraintId);
    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        if(trigger == Character.Trigger.Duck)
        {
            shouldStandUp = false;
            return true;
        }

        return false;
    }
}