using System.Collections.Generic;
using UnityEngine;

public class TribalCheckActivablesState : TribalHSMState
{
    private List<IActivable> activableStorage;

    public TribalCheckActivablesState(byte stateId, string debugName = null) : base(stateId, debugName)
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
                Blackboard.activable = collectableObject;
                if(SendEvent(Character.Order.Collect))
                {
                    return true;
                }
            }

            if (activableStorage.ContainsType<MovableObject>(out MovableObject movableObject))
            {
                Blackboard.activable = movableObject;
                if (SendEvent(Character.Order.Push))
                {
                    return true;
                }
            }

            if (activableStorage.ContainsType<Hide>(out Hide hide))
            {
                Blackboard.activable = hide;
                if(SendEvent(Character.Order.Hide))
                {
                    return true;
                }
            }

            activableStorage.ContainsType<ContextualActivable>(out ContextualActivable contextualActivable);

            Blackboard.activable = contextualActivable;

            if (SendEvent(Character.Order.Activate))
            {
                return true;
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
}