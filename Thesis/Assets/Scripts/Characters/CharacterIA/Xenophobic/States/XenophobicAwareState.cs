using SAM.FSM;
using System;
using UnityEngine;

public class XenophobicAwareState : XenophobicIAState
{
    private Vector2 lastPosition;
    private bool positionReached;
    private float positionReachedMarginX = 2;
    private float positionReachedMarginY = 4;

    private Action<Collider2D> updatePositionDelegate;

    private Eyes characterEyes;

    public XenophobicAwareState(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state, controller, blackboard)
    {
        updatePositionDelegate = UpdatePosition;

        characterEyes = controller.SlaveEyes;
    }

    protected override void OnEnter()
    {
        lastPosition = blackboard.seekedLastPosition;

        if (characterEyes != null)
        {
            characterEyes.onDistantVisionStay += updatePositionDelegate;
        }
    }

    protected override void OnUpdate()
    {
        if(IsPositionReached(lastPosition) == false)
        {
            SearchAtPosition(lastPosition);
        }
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
        if (position.x < controller.transform.position.x)
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

    private void UpdatePosition(Collider2D collider)
    {
        lastPosition = collider.transform.position;
    }
}
