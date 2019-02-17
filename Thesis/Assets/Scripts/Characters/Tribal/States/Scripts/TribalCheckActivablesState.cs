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

            IActivable<Tribal> activable = Owner.CheckForActivableObject<IActivable<Tribal>>();

            if(activable != null)
            {
                if (activable is Hide hide)
                {
                    Blackboard.toHidePlace = hide;
                    SendEvent(Character.Trigger.Hide);
                }
                else
                {
                    activable.Activate(Owner);
                }
            }
        }

        return false;
    }
}