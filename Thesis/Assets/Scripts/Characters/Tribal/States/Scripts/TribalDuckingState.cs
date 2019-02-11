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

        float yFloor = character.Collider.bounds.center.y - character.Collider.bounds.extents.y;
        
        previousSizeX = character.Collider.GetSize().x;
        previousSizeY = character.Collider.GetSize().y;

        previousOffsetX = character.Collider.offset.x;
        previousOffsetY = character.Collider.offset.y;
        
        colliderSizeY = previousSizeY / 2;

        if(colliderSizeY < previousSizeX)
        {
            colliderSizeY = previousSizeX;
        }
        
        character.Collider.ChangeSize(new Vector2(previousSizeX, colliderSizeY));
        character.Collider.offset = new Vector2(previousOffsetX, yFloor + colliderSizeY);

        velocityContraintId = character.AddVelocityConstraintByPercentageAndGetConstraintId(velocityConstraintPercentage);

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

        character.Collider.ChangeSize(new Vector2(previousSizeX, previousSizeY));
        character.Collider.offset = new Vector2(previousOffsetX, previousOffsetY);

        character.RemoveVelocityConstraintById(velocityContraintId);
    }

    protected override TriggerResponse HandleEvent(Character.Trigger trigger)
    {
        if(trigger == Character.Trigger.Duck)
        {
            
            shouldStandUp = false;
            return TriggerResponse.Reject;
        }

        return TriggerResponse.Accept;
    }
}