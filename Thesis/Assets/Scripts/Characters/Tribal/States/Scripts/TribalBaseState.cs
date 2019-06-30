using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalBaseState : TribalHSMState
{
    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        Initialize();
    }

    private void Initialize()
    {
        Owner.MaxVelocity.SetBaseValue(Configuration.MaxMovementVelocity);
    }
}