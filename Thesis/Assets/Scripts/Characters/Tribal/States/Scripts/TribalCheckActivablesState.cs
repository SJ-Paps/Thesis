using System.Collections.Generic;
using UnityEngine;

public class TribalCheckActivablesState : TribalHSMState
{
    public TribalCheckActivablesState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
    {

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
        if(CacheActivablesAndGetCount() > 0)
        {
            bool activableHasBeenAccepted = SendEvent(Character.Order.Collect)
                                        || SendEvent(Character.Order.Push)
                                        || SendEvent(Character.Order.Hide)
                                        || SendEvent(Character.Order.Activate);
            
            return activableHasBeenAccepted;
        }

        EditorDebug.Log("NO ACTIVABLES WERE FOUND");

        return false;
    }

    private int CacheActivablesAndGetCount()
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
                                    new Vector2(ownerBounds.extents.x, ownerBounds.size.y * 2), Owner.transform.eulerAngles.z, Blackboard.CurrentFrameActivables);

        return Blackboard.CurrentFrameActivables.Count;
    }
}