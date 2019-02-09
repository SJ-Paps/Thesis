using UnityEngine;

public class TribalPushingObjectState : TribalHSMState
{
    private MovableObject targetMovableObject;

    private bool shouldKeepPushing;
    private bool definitelyShouldStopPushing;

    public TribalPushingObjectState(Character.State state, string debugName) : base(state, debugName)
    {
        activeDebug = true;
    }

    protected override void OnEnter() {
        base.OnEnter();
        
        targetMovableObject = blackboard.toPushMovableObject;

        if(targetMovableObject == null)
        {
            float pushCheckDistance = 0.2f;

            character.IsMovableObjectNear(pushCheckDistance, out targetMovableObject);
        }

        if(targetMovableObject != null)
        {
            targetMovableObject.Connect(character.RigidBody2D);

            character.blockFacing = true;

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

        character.blockFacing = false;

        if(targetMovableObject != null)
        {
            targetMovableObject.Disconnect();
            targetMovableObject = null;
        }

        blackboard.toPushMovableObject = null;
    }

    protected override TriggerResponse HandleEvent(Character.Trigger trigger)
    {
        if(trigger == Character.Trigger.Push)
        {

            shouldKeepPushing = true;
        }
        if(trigger == Character.Trigger.StopPushing)
        {
            if(definitelyShouldStopPushing)
            {
                return TriggerResponse.Accept;
            }
            else
            {
                return TriggerResponse.Reject;
            }
        }

        return TriggerResponse.Accept;
    }
}
