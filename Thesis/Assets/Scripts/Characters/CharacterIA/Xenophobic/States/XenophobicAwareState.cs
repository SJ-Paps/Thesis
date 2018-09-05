using SAM.FSM;
using System;
using UnityEngine;

public class XenophobicAwareState : XenophobicIAState
{
    private Vector2 lastPosition;
    private Action<Character> onPlayerDetected;

    public XenophobicAwareState(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, Xenophobic controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state, controller, blackboard)
    {

    }

    protected override void OnEnter()
    {
        lastPosition = blackboard.seekedLastPosition;
    }

    protected override void OnUpdate()
    {
        SearchAtPosition(lastPosition);
    }

    protected override void OnExit()
    {

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
}
