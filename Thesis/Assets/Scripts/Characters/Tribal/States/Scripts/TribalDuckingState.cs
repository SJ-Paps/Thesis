﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalDuckingState : TribalHSMState
{
    private Vector2 previousOffset;
    private Vector2 previousSize;

    private float velocityConstraintPercentage = 60;

    private int velocityContraintId;

    private bool shouldStandUp;

    public TribalDuckingState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        float yFloor = Owner.Collider.bounds.min.y;

        previousOffset = Owner.Collider.offset;
        previousSize = Owner.Collider.GetSize();
        
        float colliderSizeY = previousSize.y / 2;

        if(colliderSizeY < previousSize.x)
        {
            colliderSizeY = previousSize.x;
        }

        Owner.Collider.ChangeSize(new Vector2(previousSize.x, colliderSizeY));

        float distanceFromZeroOffsetToFloor = Mathf.Abs((Owner.transform.position.y) - yFloor);

        Owner.Collider.offset = new Vector2(previousOffset.x, (previousOffset.y - distanceFromZeroOffsetToFloor / 2));

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

        /*Owner.Collider.ChangeSize(new Vector2(previousSizeX, previousSizeY));
        Owner.Collider.offset = new Vector2(previousOffsetX, previousOffsetY);*/

        Owner.Collider.ChangeSize(previousSize);
        Owner.Collider.offset = previousOffset;

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