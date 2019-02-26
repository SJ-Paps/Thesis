using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalCheckActivablesState : TribalHSMState
{
    public TribalCheckActivablesState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        if(trigger == Character.Trigger.Activate)
        {
            MovableObject movableObject = Owner.CheckForMovableObject();

            if(movableObject != null)
            {
                Blackboard.toPushMovableObject = movableObject;
                SendEvent(Character.Trigger.Push);
                return true;
            }

            IActivable<IHandOwner> activable = Owner.CheckForActivableObject<IActivable<IHandOwner>>();

            if(activable != null)
            {
                if (activable is Hide hide)
                {
                    Blackboard.toHidePlace = hide;
                    SendEvent(Character.Trigger.Hide);
                    return true;
                }
                else if(activable is CollectableObject collectable)
                {
                    Debug.Log("AAA");
                    Owner.GetHand().CollectObject(collectable);
                }
            }
        }

        return false;
    }
}