using System.Collections.Generic;
using UnityEngine;

public class TribalCheckActivablesState : TribalHSMState
{
    private List<IActivable<Tribal>> activablesStorage;

    public TribalCheckActivablesState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        activablesStorage = new List<IActivable<Tribal>>();
    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        if(trigger == Character.Trigger.Activate)
        {
            FindActivables();

            return WillUseActivable();
        }

        return false;
    }

    private void FindActivables()
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

        SJUtil.FindActivables<IActivable<Tribal>, Tribal>(new Vector2(ownerBounds.center.x + (ownerBounds.extents.x * xDirection), ownerBounds.center.y),
                                    new Vector2(ownerBounds.extents.x / 2, ownerBounds.size.y), Owner.transform.eulerAngles.z, activablesStorage);
    }

    private bool WillUseActivable()
    {
        for (int i = 0; i < activablesStorage.Count; i++)
        {
            IActivable<Tribal> activable = activablesStorage[i];
            
            if (activable is CollectableObject collectable)
            {
                Owner.GetHand().CollectObject(collectable);
                return true;
            }
            else if (activable is MovableObject movable)
            {
                Blackboard.toPushMovableObject = movable;
                SendEvent(Character.Trigger.Push);
                return true;
            }
            else if (activable is Hide hide)
            {
                Blackboard.toHidePlace = hide;
                SendEvent(Character.Trigger.Hide);
                return true;
            }
        }

        return false;
    }
}