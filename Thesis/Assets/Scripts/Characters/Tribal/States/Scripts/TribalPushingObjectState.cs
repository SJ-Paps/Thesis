using UnityEngine;

public class TribalPushingObjectState : TribalHSMState
{
    private MovableObject targetMovableObject;

    private bool shouldKeepPushing;
    private bool definitelyShouldStopPushing;

    private SpringJoint2D springJoint2d;

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        Rigidbody2D rigidbody2D = Configuration.RigidBody2D;

        springJoint2d = rigidbody2D.gameObject.AddComponent<SpringJoint2D>();
        springJoint2d.enabled = false;
        springJoint2d.autoConfigureDistance = false;
        springJoint2d.distance = 0.2f;
        springJoint2d.dampingRatio = 1;
        springJoint2d.enableCollision = true;
    }

    protected override void OnEnter() {
        base.OnEnter();

        targetMovableObject = Blackboard.GetItemOf<IActivable>("Activable") as MovableObject;

        Blackboard.UpdateItem<IActivable>("Activable", null);

        if (targetMovableObject != null)
        {
            springJoint2d.enabled = true;
            springJoint2d.connectedBody = targetMovableObject.Rigidbody2D;

            Owner.blockFacing = true;

            shouldKeepPushing = true;
        }
        else
        {
            definitelyShouldStopPushing = true;
            SendEvent(Character.Order.StopPushing);
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(shouldKeepPushing == false)
        {
            definitelyShouldStopPushing = true;
            SendEvent(Character.Order.StopPushing);
        }

        shouldKeepPushing = false;
    }

    protected override void OnExit() {
        base.OnExit();

        definitelyShouldStopPushing = false;
        shouldKeepPushing = false;

        Owner.blockFacing = false;

        if(targetMovableObject != null)
        {
            springJoint2d.enabled = false;
            springJoint2d.connectedBody = null;

            targetMovableObject = null;
        }
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if(trigger == Character.Order.Push)
        {
            shouldKeepPushing = true;
        }
        if(trigger == Character.Order.StopPushing)
        {
            if(definitelyShouldStopPushing)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        return false;
    }
}
