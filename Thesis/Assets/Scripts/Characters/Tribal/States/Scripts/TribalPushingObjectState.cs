using UnityEngine;

public class TribalPushingObjectState : TribalHSMState
{
    private MovableObject targetMovableObject;

    private bool shouldKeepPushing;
    private bool definitelyShouldStopPushing;

    public TribalPushingObjectState(Character.State state, string debugName) : base(state, debugName)
    {

    }

    protected override void OnEnter() {
        base.OnEnter();
        
        targetMovableObject = Blackboard.toPushMovableObject;

        if(targetMovableObject == null)
        {
            targetMovableObject = Owner.CheckForMovableObject();
        }

        if(targetMovableObject != null)
        {
            targetMovableObject.Activate(Owner);

            Owner.blockFacing = true;

            shouldKeepPushing = true;
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(shouldKeepPushing == false)
        {
            definitelyShouldStopPushing = true;
            SendEvent(Character.Trigger.StopPushing);
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
            targetMovableObject.Activate(Owner);
            targetMovableObject = null;
        }

        Blackboard.toPushMovableObject = null;
    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        if(trigger == Character.Trigger.Push)
        {
            shouldKeepPushing = true;
        }
        if(trigger == Character.Trigger.StopPushing)
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
