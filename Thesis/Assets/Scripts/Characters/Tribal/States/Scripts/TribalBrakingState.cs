using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalBrakingState : TribalHSMState
{
    private bool isMovingByWill;

    public TribalBrakingState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (isMovingByWill)
        {
            SendEvent(Character.Trigger.Move);
        }

        isMovingByWill = false;
    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        if (trigger == Character.Trigger.MoveLeft || trigger == Character.Trigger.MoveRight)
        {
            isMovingByWill = true;
        }

        return false;
    }
}