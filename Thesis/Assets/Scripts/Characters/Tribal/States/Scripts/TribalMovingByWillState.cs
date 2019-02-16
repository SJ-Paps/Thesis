using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalMovingByWillState : CharacterHSMState
{
    private bool isMovingByWill;

    public TribalMovingByWillState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        isMovingByWill = true;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(isMovingByWill == false)
        {
            SendEvent(Character.Trigger.StopMoving);
        }

        isMovingByWill = false;
    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        if(trigger == Character.Trigger.MoveLeft || trigger == Character.Trigger.MoveRight)
        {
            isMovingByWill = true;
        }

        return false;
    }


}