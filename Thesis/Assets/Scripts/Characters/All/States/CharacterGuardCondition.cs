using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterGuardCondition : GuardCondition, IOwnable<Character>
{
    protected Character character;

    public Character Owner
    {
        get
        {
            return character;
        }
    }

    public void PropagateOwnerReference(Character ownerReference)
    {
        character = ownerReference;

        OnOwnerReferencePropagated();
    }

    protected virtual void OnOwnerReferencePropagated()
    {

    }
}
