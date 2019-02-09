using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalChoiceIdleOrMovingState : TribalHSMState
{
    public TribalChoiceIdleOrMovingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        Redirect();
    }

    private void Redirect()
    {
        if(character.RigidBody2D.velocity.x > 0)
        {
            SendEvent(Character.Trigger.MoveRight);
        }
        else if(character.RigidBody2D.velocity.x < 0)
        {
            SendEvent(Character.Trigger.MoveLeft);
        }
        else
        {
            SendEvent(Character.Trigger.StopMoving);
        }
    }
}