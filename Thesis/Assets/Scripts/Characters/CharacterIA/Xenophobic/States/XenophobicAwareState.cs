using SAM.FSM;
using SAM.Timers;
using System;
using UnityEngine;

public class XenophobicAwareState : XenophobicIAState
{
    private Vector2 lastPosition;
    private bool positionReached;
    private float positionReachedMarginX = 1;
    private float positionReachedMarginY = 2;

    private Vector2 distantVisionSize = new Vector2(10, 10);

    private Action<Collider2D> updatePositionDelegate;

    private Eyes characterEyes;

    private SyncTimer searchTimer;
    private float searchInterval = 4f;

    public XenophobicAwareState(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state, controller, blackboard)
    {
        updatePositionDelegate = UpdatePosition;

        characterEyes = controller.SlaveEyes;

        searchTimer = new SyncTimer();
        searchTimer.Interval = searchInterval;
        searchTimer.onTick += CalmDown;
    }

    protected override void OnEnter()
    {
        if (characterEyes != null)
        {
            characterEyes.DistantVision.ChangeSize(distantVisionSize);
            characterEyes.DistantVision.InnerCollider.offset = new Vector2(0, characterEyes.DistantVision.InnerCollider.offset.y);

            characterEyes.onDistantVisionStay += updatePositionDelegate;
        }

        UpdatePosition(blackboard.seekedLastPosition);
    }

    protected override void OnUpdate()
    {
        if(IsPositionReached(lastPosition) == false)
        {
            SearchAtPosition(lastPosition);
        }

        searchTimer.Update(Time.deltaTime);
    }

    protected override void OnExit()
    {
        if (characterEyes != null)
        {
            characterEyes.onDistantVisionStay -= updatePositionDelegate;
        }
    }

    private void SearchAtPosition(Vector2 position)
    {
        if (position.x < controller.Slave.transform.position.x)
        {
            controller.Slave.SetOrder(Character.Order.OrderMoveLeft);
        }
        else
        {
            controller.Slave.SetOrder(Character.Order.OrderMoveRight);
        }
    }

    private bool IsPositionReached(Vector2 position)
    {
        Bounds b = new Bounds(position, new Vector2(positionReachedMarginX * 2, positionReachedMarginY * 2));

        if (b.Contains(controller.Slave.transform.position))
        {
            return true;
        }

        return false;
    }

    private void UpdatePosition(Vector2 position)
    {
        lastPosition = position;
        searchTimer.Start();
    }

    private void UpdatePosition(Collider2D collider)
    {
        UpdatePosition(collider.transform.position);
    }

    private void CalmDown(SyncTimer timer)
    {
        CalmDown();
    }

    private void CalmDown()
    {
        stateMachine.Trigger(XenophobicIAController.Trigger.CalmDown);
    }
}
