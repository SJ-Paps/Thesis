using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalCheckPushable : TribalGuardCondition
{
    public override bool IsValid()
    {
        return IsPushableReachable();
    }

    private bool IsPushableReachable()
    {
        Vector2 detectionSize = new Vector2((character.Collider.bounds.extents.x * 2) + Tribal.movableObjectDetectionOffset, character.Collider.bounds.extents.y * 2);

        MovableObject movableObject = SJUtil.FindActivable<MovableObject, Character>(character.Collider.bounds.center, detectionSize, character.transform.eulerAngles.z);

        if (movableObject != null)
        {
            blackboard.toPushMovableObject = movableObject;
            return true;
        }

        return false;

    }
}
