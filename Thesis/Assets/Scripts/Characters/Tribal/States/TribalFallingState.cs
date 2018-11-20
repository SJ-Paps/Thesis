using SAM.FSM;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TribalFallingState : CharacterState
{
    [SerializeField]
    private float circlecastRadius = 0.1f;

    private int ledgeLayer;

    private Animator animator;

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orderList, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orderList, blackboard);

        animator = character.Animator;

        ledgeLayer = 1 << Reg.ledgeLayer;
    }

    protected override void OnEnter()
    {
        animator.SetTrigger("Fall");

        EditorDebug.Log("FALLING ENTER " + character.name);
    }

    protected override void OnUpdate()
    {
        CheckingForLedge();

        if(character.IsOnFloor(Reg.walkableLayerMask))
        {
            stateMachine.Trigger(Character.Trigger.Ground);
            return;
        }
    }

    protected override void OnExit()
    {
        animator.ResetTrigger("Fall");
    }

    private void CheckingForLedge()
    {
        Bounds bounds = character.Collider.bounds;
        float xDir = character.transform.right.x;

        Vector2 detectionPoint = new Vector2(bounds.center.x + ((bounds.extents.x + 0.1f) * xDir), bounds.center.y + bounds.extents.y);

        Collider2D auxColl = Physics2D.OverlapPoint(detectionPoint, ledgeLayer);

        if (auxColl != null)
        {
            blackboard.LastLedgeDetected = auxColl;
            stateMachine.Trigger(Character.Trigger.Grapple);
        }
    }

}
