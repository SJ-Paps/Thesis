using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalHangingRopeState : TribalHSMState
{
    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        Blackboard.UpdateItem<RelativeJoint2DTuple>("RopeHandler", new GameObject("RopeHandler").AddComponent<RelativeJoint2DTuple>());
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Rope rope = (Rope)Blackboard.GetItemOf<IActivable>("Activable");

        Owner.RigidBody2D.velocity = new Vector2(0, 0);

        Rigidbody2D nearestSegment = rope.GetNearestSegment(Owner.transform.position);

        nearestSegment.velocity = new Vector2(0, 0);

        RelativeJoint2DTuple ropeHandler = Blackboard.GetItemOf<RelativeJoint2DTuple>("RopeHandler");
        

        ropeHandler.Rigidbody2D.MovePosition(nearestSegment.position);
        
        ropeHandler.Connect(Owner.RigidBody2D, nearestSegment);
        
        ropeHandler.RelativeMe.autoConfigureOffset = false;
        ropeHandler.RelativeOther.autoConfigureOffset = false;
        
        ropeHandler.RelativeMe.maxForce = 300;
        ropeHandler.RelativeOther.maxForce = 300;
        
        ropeHandler.RelativeMe.linearOffset = new Vector2(0, 0);
        ropeHandler.RelativeOther.linearOffset = new Vector2(0, 0);
    }

    protected override void OnExit()
    {
        base.OnExit();

        Blackboard.GetItemOf<RelativeJoint2DTuple>("RopeHandler").Disconnect();
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if(trigger == Character.Order.Jump)
        {
            SendEvent(Character.Order.Fall);
            return true;
        }

        return false;
    }
}
