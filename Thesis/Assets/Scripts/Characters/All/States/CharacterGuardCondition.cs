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

    protected Character.Blackboard blackboard;

    public void PropagateOwnerReference(Character ownerReference)
    {
        character = ownerReference;

        OnOwnerReferencePropagated();
    }

    public void PropagateBlackboardReference(Character.Blackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    protected virtual void OnOwnerReferencePropagated()
    {

    }
}
