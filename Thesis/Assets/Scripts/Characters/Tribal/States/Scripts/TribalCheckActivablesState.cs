using System.Collections.Generic;
using UnityEngine;

public class TribalCheckActivablesState : TribalHSMState
{
    private List<IActivable> activablesStorage;

    public TribalCheckActivablesState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        activablesStorage = new List<IActivable>();
    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        if(trigger == Character.Trigger.Activate)
        {
            FindActivables();

            bool aux = WillActivateObject();

            activablesStorage.Clear();

            return aux;
        }
        else if(trigger == Character.Trigger.Drop)
        {
            return Owner.GetHand().DropObject();
        }
        else if(trigger == Character.Trigger.Throw)
        {
            return Owner.GetHand().ThrowObject();
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

        SJUtil.FindActivables(new Vector2(ownerBounds.center.x + (ownerBounds.extents.x * xDirection), ownerBounds.center.y),
                                    new Vector2(ownerBounds.extents.x, ownerBounds.size.y * 2), Owner.transform.eulerAngles.z, activablesStorage);
    }

    private bool WillActivateObject()
    {
        for(int i = 0; i < activablesStorage.Count; i++)
        {
            IActivable activable = activablesStorage[i];

            if(activable is CollectableObject collectable)
            {
                if(Owner.GetHand().CollectObject(collectable))
                {
                    return true;
                }
            }
            else if(activable is MovableObject movable)
            {
                Blackboard.toPushMovableObject = movable;
                SendEvent(Character.Trigger.Push);
                return true;
            }
            else if(activable is Hide hide)
            {
                Blackboard.toHidePlace = hide;
                SendEvent(Character.Trigger.Hide);
                return true;
            }
        }

        if(Owner.GetHand().IsFree == false)
        {
            CollectableObject objectInHand = Owner.GetHand().CurrentCollectable;

            if(objectInHand is Weapon)
            {
                SendEvent(Character.Trigger.Attack);
            }
            else
            {
                Owner.GetHand().ActivateCurrentObject();
            }
        }

        return false;
    }
}