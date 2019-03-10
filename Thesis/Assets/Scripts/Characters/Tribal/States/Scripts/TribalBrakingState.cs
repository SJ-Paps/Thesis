﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalBrakingState : TribalHSMState
{
    private bool isMovingByWill;

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (isMovingByWill)
        {
            SendEvent(Character.Order.Move);
        }

        isMovingByWill = false;
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if (trigger == Character.Order.MoveLeft || trigger == Character.Order.MoveRight)
        {
            isMovingByWill = true;
        }

        return false;
    }
}