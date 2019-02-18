using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalCheckIsInFrontOfHide : TribalGuardCondition
{
    protected override bool Validate()
    {
        Vector2 detectionSize = new Vector2((Owner.Collider.bounds.extents.x * 2) + Tribal.activableDetectionOffset, Owner.Collider.bounds.extents.y * 2);

        Hide hide = SJUtil.FindActivable<Hide, Tribal>(Owner.Collider.bounds.center, detectionSize, Owner.transform.eulerAngles.z);

        if(hide != null)
        {
            
            Blackboard.toHidePlace = hide;
            return true;
        }

        return false;
    }
}
