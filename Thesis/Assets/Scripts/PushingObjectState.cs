using SAM.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingObjectState : CharacterState {

    private Action<Collision2D> checkingForExitingOfPushingObjectMethod;
    private RaycastHit2D raycastHit2D;
    private float raycastDistance;

    public PushingObjectState(
       FSM<Character.State, Character.Trigger> fsm,
       Character.State state,
       Character character,
       List<Character.Order> orders,
       Character.Blackboard blackboard,
       FSM<Character.State, Character.Trigger> jumpingFSM,
       FSM<Character.State, Character.Trigger> movementFSM) : base (fsm, state, character, orders, blackboard)
    {
        raycastDistance = 0.4f;
    }

    protected override void OnEnter() {
        base.OnEnter();
        EditorDebug.Log("PUSHINGOBJECT ENTER");
    }

    protected override void OnExit() {
        base.OnExit();
    }

    protected override void OnUpdate() {
        raycastHit2D = Physics2D.Raycast(character.transform.position, Vector2.right * character.transform.localPosition.x, raycastDistance);

        EditorDebug.DrawLine(character.transform.position, raycastHit2D.point, Color.red);

        if(raycastHit2D.collider == null)
        {
            stateMachine.Trigger(Character.Trigger.StopPushing);
        }

        for(int i = 0; i < orders.Count; i++)
        {
            Character.Order ev = orders[i];

            if(ev == Character.Order.OrderPush)
            {
                EditorDebug.Log("EMPUJO OBJETO");
            }
        }

        base.OnUpdate();
    }
}
