using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHSMState : CharacterHSMState
{
    public new Turret Owner { get; protected set; }
    

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        Owner = (Turret)base.Owner;
    }
}