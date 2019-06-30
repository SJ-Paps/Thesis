﻿using System.Collections.Generic;
using UnityEngine;

public class TribalCheckActivablesState : TribalHSMState
{
    private List<IActivable> activableStorage;

    public TribalCheckActivablesState()
    {
        activableStorage = new List<IActivable>();
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if(trigger == Character.Order.SwitchActivables)
        {
            return SwitchActivables();
        }

        return false;
    }

    private bool SwitchActivables()
    {
        CacheActivables();

        if (activableStorage.Count > 0)
        {
            if(activableStorage.ContainsType<CollectableObject>(out CollectableObject collectableObject))
            {

                Blackboard.UpdateItem<IActivable>("Activable", collectableObject);
                if(SendEvent(Character.Order.Collect))
                {
                    return true;
                }
            }

            if (activableStorage.ContainsType<MovableObject>(out MovableObject movableObject))
            {
                Blackboard.UpdateItem<IActivable>("Activable", movableObject);
                if (SendEvent(Character.Order.Push))
                {
                    return true;
                }
            }

            if (activableStorage.ContainsType<Hide>(out Hide hide))
            {
                Blackboard.UpdateItem<IActivable>("Activable", hide);
                if(SendEvent(Character.Order.Hide))
                {
                    return true;
                }
            }

            if(SwitchClimbables())
            {
                return true;
            }

            if(activableStorage.ContainsType<ContextualActivable>(out ContextualActivable contextualActivable)
                || activableStorage.ContainsType<CollectableObject>(out collectableObject))
            {
                Blackboard.UpdateItem<IActivable>("Activable", contextualActivable);

                if (SendEvent(Character.Order.Activate))
                {
                    return true;
                }
            }
        }

        EditorDebug.Log("NO ACTIVABLES WERE FOUND");

        return false;
    }

    private void CacheActivables()
    {
        activableStorage.Clear();

        Owner.FindActivables(activableStorage);
    }

    private bool SwitchClimbables()
    {
        if (activableStorage.ContainsType<Ladder>(out Ladder ladder))
        {
            Blackboard.UpdateItem<IActivable>("Activable", ladder);
            if (SendEvent(Character.Order.HangLadder))
            {
                return true;
            }
        }

        if (activableStorage.ContainsType<Rope>(out Rope rope))
        {
            Blackboard.UpdateItem<IActivable>("Activable", rope);
            if (SendEvent(Character.Order.HangRope))
            {
                return true;
            }
        }

        if (activableStorage.ContainsType<ClimbableWall>(out ClimbableWall climbableWall))
        {
            Blackboard.UpdateItem<IActivable>("Activable", climbableWall);
            if (SendEvent(Character.Order.HangWall))
            {
                return true;
            }
        }

        return false;
    }
}