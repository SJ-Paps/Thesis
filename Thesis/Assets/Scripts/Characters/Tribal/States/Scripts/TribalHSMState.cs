using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalHSMState : CharacterHSMState
{
    protected new Tribal character;

    public TribalHSMState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnOwnerReferencePropagated()
    {
        character = (Tribal)base.character;
    }
}