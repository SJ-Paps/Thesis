using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalMovingByWillState : TribalHSMState
{
    private bool isMovingByWill;

    public TribalMovingByWillState(Tribal.State stateId, string debugName = null) : base(stateId, debugName)
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
            SendEvent(Character.Order.StopMoving);
        }

        isMovingByWill = false;
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if(trigger == Character.Order.MoveLeft || trigger == Character.Order.MoveRight)
        {
            isMovingByWill = true;
        }

        return false;
    }


}