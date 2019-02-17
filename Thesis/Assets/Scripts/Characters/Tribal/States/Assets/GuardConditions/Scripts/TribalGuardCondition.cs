using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TribalGuardCondition : CharacterGuardCondition
{
    protected new Tribal Owner;

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        Owner = (Tribal)base.Owner;
    }
}
