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
        float pushCheckDistance = 0.2f;

        MovableObject movableObject;

        if(character.IsMovableObjectNear(pushCheckDistance, out movableObject))
        {
            blackboard.toPushMovableObject = movableObject;
            return true;
        }

        return false;

    }
}
