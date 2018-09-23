﻿using SAM.FSM;
using UnityEngine;
using System;

public class XenophobicIAController : UnityController<Xenophobic, Character.Order>
{
    public enum State
    {
        CalmedDown,
        Aware,
        FullAlert,
        Patrolling
    }

    public enum Trigger
    {
        CalmDown,
        GetAware,
        SetFullAlert,
        Patrol
    }

    public class Blackboard
    {
        public event Action<Vector2> onLastDetectionPositionChanged;

        private Vector2 lastDetectionPosition;

        public Vector2 LastDetectionPosition
        {
            get
            {
                return lastDetectionPosition;
            }

            set
            {
                lastDetectionPosition = value;

                if(onLastDetectionPositionChanged != null)
                {
                    onLastDetectionPositionChanged(lastDetectionPosition);
                }
            }
        }
    }

    protected Blackboard blackboard;

    protected FSM<State, Trigger> alertnessFSM;
    protected FSM<State, Trigger> behaviourFSM;

    public Eyes SlaveEyes { get; protected set; }

    void Start()
    {
        blackboard = new Blackboard();
        SlaveEyes = Slave.GetComponentInChildren<Eyes>();
        SlaveEyes.SetEyePoint(Slave.EyePoint);

        alertnessFSM = new FSM<State, Trigger>();

        alertnessFSM.AddState(new XenophobicAlertlessState(alertnessFSM, State.CalmedDown, this, blackboard));
        alertnessFSM.AddState(new XenophobicAwareState(alertnessFSM, State.Aware, this, blackboard));
        alertnessFSM.AddState(new XenophobicFullAlertState(alertnessFSM, State.FullAlert, this, blackboard));

        alertnessFSM.MakeTransition(State.CalmedDown, Trigger.GetAware, State.Aware);
        alertnessFSM.MakeTransition(State.Aware, Trigger.SetFullAlert, State.FullAlert);
        alertnessFSM.MakeTransition(State.FullAlert, Trigger.CalmDown, State.Aware);
        alertnessFSM.MakeTransition(State.Aware, Trigger.CalmDown, State.CalmedDown);

        alertnessFSM.StartBy(State.CalmedDown);


        behaviourFSM = new FSM<State, Trigger>();

        behaviourFSM.AddState(new XenophobicPatrol(behaviourFSM, State.Patrolling, this, blackboard));

        behaviourFSM.StartBy(State.Patrolling);
    }

    public override void Control()
    {
        alertnessFSM.UpdateCurrentState();
        behaviourFSM.UpdateCurrentState();
    }

    void Update()
    {
        if(Slave != null)
        {
            Control();
        }
    }

}
