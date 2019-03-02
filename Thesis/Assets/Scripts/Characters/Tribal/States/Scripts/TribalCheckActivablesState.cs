using System.Collections.Generic;
using UnityEngine;

public class TribalCheckActivablesState : TribalHSMState
{
    private List<IActivable> activableStorage;

    public TribalCheckActivablesState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
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
            else if(activableStorage.ContainsType<MovableObject>(out MovableObject movableObject))
            {
                Blackboard.activable = movableObject;
                if (SendEvent(Character.Order.Push))
                {
                    return true;
                }
            }
            else if(activableStorage.ContainsType<Hide>(out Hide hide))
            {
                Blackboard.activable = hide;
                if(SendEvent(Character.Order.Hide))
                {
                    return true;
                }
            }

            activableStorage.ContainsType<ContextualActivable>(out ContextualActivable contextualActivable);

            Blackboard.activable = contextualActivable;

            SendEvent(Character.Order.Activate);

            activableStorage.Clear();
        }

        EditorDebug.Log("NO ACTIVABLES WERE FOUND");

        return false;
    }

    private void CacheActivables()
    {
        Bounds ownerBounds = Owner.Collider.bounds;

        int xDirection;

        if (Owner.IsFacingLeft)
        {
            xDirection = -1;
        }
        else
        {
            xDirection = 1;
        }

        //guardo los activables en la lista del blackboard
        SJUtil.FindActivables(new Vector2(ownerBounds.center.x + (ownerBounds.extents.x * xDirection), ownerBounds.center.y),
                                    new Vector2(ownerBounds.extents.x, ownerBounds.size.y * 2), Owner.transform.eulerAngles.z, activableStorage);
    }
}