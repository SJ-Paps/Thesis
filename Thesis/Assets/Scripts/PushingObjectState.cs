using SAM.FSM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingObjectState : CharacterState {

    private RaycastHit2D raycastHit2D;
    private float raycastDistance;
    private bool jointObtained;
    private FixedJoint2D objectFixedJoint2D;
    private Rigidbody2D characterRigidbody;

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
        jointObtained = false;
        characterRigidbody = character.GetComponent<Rigidbody2D>();
    }

    protected override void OnEnter() {
        base.OnEnter();
        EditorDebug.Log("PUSHINGOBJECT ENTER");
    }

    protected override void OnExit() {
        base.OnExit();
        EditorDebug.Log("PUSHINGOBJECT EXIT");
    }

    protected override void OnUpdate() {

        raycastHit2D = Physics2D.Raycast(character.transform.position, (Vector2)character.transform.right, raycastDistance);
        EditorDebug.DrawLine(character.transform.position, (Vector2)character.transform.localPosition + (Vector2)character.transform.right * raycastDistance, Color.red);

        if(!jointObtained && character.IsGrounded)
        {
            objectFixedJoint2D = raycastHit2D.transform.GetComponent<FixedJoint2D>();
            jointObtained = true;
        }
        if(!raycastHit2D || raycastHit2D.collider.gameObject.layer != Reg.objectLayer || !character.IsGrounded)
        { 
            blackboard.isPushing = false;
            jointObtained = false;
            objectFixedJoint2D.enabled = false;
            stateMachine.Trigger(Character.Trigger.StopPushing);
        }

        for(int i = 0; i < orders.Count; i++)
        {
            Character.Order ev = orders[i];

            PushOrPullObjectWhenPressingKey(ev);
        }

        base.OnUpdate();
    }

    private void PushOrPullObjectWhenPressingKey(Character.Order order) 
    {
        if(order == Character.Order.OrderAction)
        {
            EditorDebug.Log("EMPUJO OBJETO");
            objectFixedJoint2D.enabled = true;
            objectFixedJoint2D.connectedBody = characterRigidbody;
        }
        else
        {
            objectFixedJoint2D.connectedBody = null;
            objectFixedJoint2D.enabled = false;
        }
        if(order == Character.Order.OrderAction && !character.IsGrounded)
        {
            objectFixedJoint2D.connectedBody = null;
            blackboard.isPushing = false;
            jointObtained = false;
            objectFixedJoint2D.enabled = false;
            stateMachine.Trigger(Character.Trigger.StopPushing);
        }
    }
}
