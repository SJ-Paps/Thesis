using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TribalGuardCondition : CharacterGuardCondition
{
    protected new Tribal character;

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        character = (Tribal)base.character;
    }
}
