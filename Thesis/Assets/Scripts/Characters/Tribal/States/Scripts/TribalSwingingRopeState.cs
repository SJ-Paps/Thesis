using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TribalSwingingRopeState : TribalHSMState
{
    protected override void OnEnter()
    {
        base.OnEnter();

        RelativeJoint2DTuple ropeHandler = Blackboard.GetItemOf<RelativeJoint2DTuple>("RopeHandler");

        if (LastEnteringTrigger == Character.Order.MoveLeft)
        {
            Owner.Face(true);
            ropeHandler.RelativeOther.connectedBody.AddRelativeForce(new Vector2(-Owner.TribalConfigurationData.ClimbForce * 6, 0));
        }
        else if (LastEnteringTrigger == Character.Order.MoveRight)
        {
            Owner.Face(false);
            ropeHandler.RelativeOther.connectedBody.AddRelativeForce(new Vector2(Owner.TribalConfigurationData.ClimbForce * 6, 0));
        }

        SendEvent(Character.Order.FinishAction);
    }
}
