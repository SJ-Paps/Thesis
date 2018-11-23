using SAM.FSM;
using SAM.Timers;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

[Serializable]
public class TribalHiddenState : CharacterState 
{
    private SyncTimer timerForComingOut;

    [SerializeField]
    private float cooldownForComingOut = 0.7f;

    private Rigidbody2D rigidbody2D;

    private SortingGroup sortingGroup;

    public override void InitializeState(FSM<Character.State, Character.Trigger> fsm, Character.State state, Character character, List<Character.Order> orders, Character.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, character, orders, blackboard);
        
        timerForComingOut = new SyncTimer();

        timerForComingOut.onTick += StopTimerForComingOut;
        timerForComingOut.Interval = cooldownForComingOut;

        rigidbody2D = character.RigidBody2D;

        sortingGroup = character.GetComponentInChildren<SortingGroup>();
    }

    protected override void OnEnter() 
    {
        blackboard.isHidden = true;

        rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);

        sortingGroup.sortingOrder = 4;
        EditorDebug.Log("HIDDEN ENTER");
    }

    protected override void OnExit()
    {
        blackboard.isHidden = false;

        sortingGroup.sortingOrder = 6;
        EditorDebug.Log("HIDDEN EXIT");
    }

    protected override void OnUpdate() 
    {
        Hide();
    }

    private void Hide() 
    {
        timerForComingOut.Update(Time.deltaTime);

        for (int i = 0; i < orders.Count; i++)
        {
            Character.Order ev = orders[i];

            if (ev == Character.Order.OrderHide && !timerForComingOut.Active) 
            {
                EditorDebug.Log("LLAMADO AL TIMER HIDDEN");
                timerForComingOut.Start();
            }
        }
    }

    private void ComingOutOfTheHidingPlace() 
    {
        stateMachine.Trigger(Character.Trigger.StopHiding);
    }

    void StopTimerForComingOut(SyncTimer timer) 
    {
        ComingOutOfTheHidingPlace();
    }
}