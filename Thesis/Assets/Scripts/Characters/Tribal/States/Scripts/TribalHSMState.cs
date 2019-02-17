using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalHSMState : CharacterHSMState
{
    public new Tribal Owner { get; set; }

    public TribalHSMState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();
        Owner = (Tribal)base.Owner;
    }
}