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
            MovableObject movableObject = character.CheckForMovableObject();

            if(movableObject != null)
            {
                blackboard.toPushMovableObject = movableObject;
                SendEvent(Character.Trigger.Push);
                return true;
            }

            IActivable<Character> activable = character.CheckForActivableObject<IActivable<Character>>();

            if(activable != null)
            {
                if (activable is Hide hide)
                {
                    blackboard.toHidePlace = hide;
                    SendEvent(Character.Trigger.Hide);
                }
                else
                {
                    activable.Activate(character);
                }
            }
        }

        return false;
    }
}