using SAM.FSM;
using System;
using UnityEngine;
using System.Collections.Generic;

public class HiddenState : CharacterState 
{
    private FSM<Character.State, Character.Trigger> characterJumpingFSM;
    private FSM<Character.State, Character.Trigger> characterMovementFSM;
    private Character.Blackboard characterBlackboard;
    private Rigidbody2D characterRigidbody2D;
    private Collider2D characterCollider2D;
    private Action<Collider2D> checkingForComingOutOfTheHidingPlaceMethod;
    private int hidingPlaceLayer;

    public HiddenState(FSM<Character.State, Character.Trigger> fsm, Character.State state,
       Character character,
       List<Character.Order> orders,
       Character.Blackboard blackboard,
       FSM<Character.State,Character.Trigger> jumpingFSM,
       FSM<Character.State, Character.Trigger> movementFSM) : base(fsm, state, character, orders)
    {
        characterJumpingFSM = jumpingFSM;
        characterMovementFSM = movementFSM;
        characterBlackboard = blackboard;
        hidingPlaceLayer = 8;
        characterCollider2D = character.GetComponent<Collider2D>();
        checkingForComingOutOfTheHidingPlaceMethod += CheckingForComingOutOfTheHidingPlace;
        characterRigidbody2D = character.GetComponent<Rigidbody2D>();
    }

    protected override void OnEnter() 
    {
        EditorDebug.Log("Entrado a Hide");
        character.onTriggerExit2D += checkingForComingOutOfTheHidingPlaceMethod;
    }

    protected override void OnExit()
    {
        EditorDebug.Log("Salí de Hide");
        character.onTriggerExit2D -= checkingForComingOutOfTheHidingPlaceMethod;
    }

    protected override void OnUpdate() 
    {
        Hide();
    }

    private void Hide() 
    {
        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order ev = orders[i];

            if (ev == Character.Order.OrderHide && character.isHidden == false) 
            {
                ComingOutOfTheHidingPlace();
            }
        }
    }

    void CheckingForComingOutOfTheHidingPlace(Collider2D collider2D)
    {
        if (collider2D.gameObject.layer == hidingPlaceLayer) 
        {
            characterBlackboard.isHiding = false;
        }
    }

    private void ComingOutOfTheHidingPlace() 
    {
        characterBlackboard.isHiding = false;
        characterCollider2D.isTrigger = false;
        characterRigidbody2D.constraints = RigidbodyConstraints2D.None;
        characterMovementFSM.Active = true;
        characterJumpingFSM.Active = true;
        stateMachine.Trigger(Character.Trigger.StopHiding);
    }
}
