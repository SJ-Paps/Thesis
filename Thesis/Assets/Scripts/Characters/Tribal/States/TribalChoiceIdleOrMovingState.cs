using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalChoiceIdleOrMovingState : TribalHSMState
{
    public TribalChoiceIdleOrMovingState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Redirect();
    }

    private void Redirect()
    {
        if(character.RigidBody2D.velocity.x > 0)
        {
            SendEvent(Character.Trigger.Move);
        }
        else
        {
            SendEvent(Character.Trigger.StopMoving);
        }
    }
}