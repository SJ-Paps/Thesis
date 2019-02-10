using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalCheckIsInFrontOfHide : TribalGuardCondition
{
    public override bool IsValid()
    {
        Vector2 detectionSize = new Vector2((character.Collider.bounds.extents.x * 2) + Tribal.activableDetectionOffset, character.Collider.bounds.extents.y * 2);

        Hide hide = SJUtil.FindActivable<Hide, Character>(character.Collider.bounds.center, detectionSize, character.transform.eulerAngles.z);

        if(hide != null)
        {
            blackboard.toHidePlace = hide;
            return true;
        }

        return false;
    }
}
