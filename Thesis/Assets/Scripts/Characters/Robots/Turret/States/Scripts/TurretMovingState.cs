using UnityEngine;
using System;

public class TurretMovingState : TurretHSMState
{
    private Action onFixedUpdateDelegate;

    private bool isOrderingMove;

    private int currentDirectionX;

    public TurretMovingState(Turret.State state, string debugName = null) : base(state, debugName)
    {
        onFixedUpdateDelegate = OnFixedUpdate;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.onFixedUpdate += onFixedUpdateDelegate;
    }

    private void OnFixedUpdate()
    {
        if(isOrderingMove)
        {
            Owner.HeadRigidBody.AddTorque(currentDirectionX * Owner.Acceleration.CurrentValueFloat, ForceMode2D.Force);
        }
        else
        {
            SendEvent(Character.Order.StopMoving);
        }

        currentDirectionX = 0;

        isOrderingMove = false;
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.onFixedUpdate -= onFixedUpdateDelegate;
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if(trigger == Character.Order.MoveLeft)
        {
            currentDirectionX = (int)Vector2.left.x * -1;
            isOrderingMove = true;
            return true;
        }
        else if(trigger == Character.Order.MoveRight)
        {
            currentDirectionX = (int)Vector2.right.x * -1;
            isOrderingMove = true;
            return true;
        }

        return false;
    }
}
