using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalCheckPushable : TribalGuardCondition
{
    protected override bool Validate()
    {
        return IsPushableReachable();
    }

    private bool IsPushableReachable()
    {
        return character.CheckForMovableObject() != null;
    }
}
