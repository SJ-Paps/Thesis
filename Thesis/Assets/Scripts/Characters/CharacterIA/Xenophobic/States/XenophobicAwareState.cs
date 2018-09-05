using SAM.FSM;
using System;
using UnityEngine;

public class XenophobicAwareState : XenophobicIAState
{
    private Vector2 lastPosition;
    private Action<Character> onPlayerDetected;
    private bool positionReached;
    private float positionReachedMarginX = 2;
    private float positionReachedMarginY = 4;

    private Action<Vector2> updatePositionDelegate;

    public XenophobicAwareState(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, Xenophobic controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state, controller, blackboard)
    {
        updatePositionDelegate = UpdatePosition;
    }

    protected override void OnEnter()
    {
        lastPosition = blackboard.seekedLastPosition;

        character.onSomethingDetected += updatePositionDelegate;
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
        character.onSomethingDetected -= updatePositionDelegate;
    }

    private void SearchAtPosition(Vector2 position)
    {
        if (position.x < character.transform.position.x)
        {
            character.SetOrder(Character.Order.OrderMoveLeft);
        }
        else
        {
            character.SetOrder(Character.Order.OrderMoveRight);
        }
    }

    private bool IsPositionReached(Vector2 position)
    {
        float positiveMarginX = position.x + positionReachedMarginX;
        float positiveMarginY = position.y + positionReachedMarginY;

        float negativeMarginX = position.x - positionReachedMarginX;
        float negativeMarginY = position.y - positionReachedMarginY;

        float characterX = character.transform.position.x;
        float characterY = character.transform.position.y;
        
        if (positiveMarginX > characterX && negativeMarginX < characterX)
        {
            if(positiveMarginY > characterY && negativeMarginY < characterY)
            {
                return true;
            }
        }

        return false;
    }

    private void UpdatePosition(Vector2 position)
    {
        lastPosition = position;
    }
}
