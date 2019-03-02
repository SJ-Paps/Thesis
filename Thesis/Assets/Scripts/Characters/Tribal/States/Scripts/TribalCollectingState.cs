using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalCollectingState : TribalHSMState
{
    public TribalCollectingState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Blackboard.CurrentFrameActivables.ContainsType<CollectableObject>(out CollectableObject collectableObject);

        if(collectableObject != null)
        {
            Owner.GetHand().CollectObject(collectableObject);
        }

        SendEvent(Character.Order.FinishAction);
    }
}
