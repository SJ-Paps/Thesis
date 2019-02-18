using UnityEngine;
using System;

public class TurretMovingState : TurretHSMState
{
    private Action onFixedUpdateDelegate;

    private bool isOrderingMove;

    private int currentDirectionX;

    public TurretMovingState(Character.State state, string debugName = null) : base(state, debugName)
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
            SendEvent(Character.Trigger.StopMoving);
        }

        currentDirectionX = 0;

        isOrderingMove = false;
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.onFixedUpdate -= onFixedUpdateDelegate;
    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        if(trigger == Character.Trigger.MoveLeft)
        {
            currentDirectionX = (int)Vector2.left.x * -1;
            isOrderingMove = true;
            return true;
        }
        else if(trigger == Character.Trigger.MoveRight)
        {
            currentDirectionX = (int)Vector2.right.x * -1;
            isOrderingMove = true;
            return true;
        }

        return false;
    }
}
