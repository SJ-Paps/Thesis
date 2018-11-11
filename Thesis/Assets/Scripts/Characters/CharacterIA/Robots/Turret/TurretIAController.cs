using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAM.FSM;
using System;

public class TurretIAController : UnityController<Turret, Character.Order>
{
    public enum State
    {
        Alertless,
        Alertful,
        WithTarget,
        WithoutTarget
    }

    public enum Trigger
    {
        CalmDown,
        SetFullAlert,
        TargetFound,
        TargetLost
    }

    public class Blackboard
    {
        public event Action<Vector2> onTargetChanged;

        private Vector2 targetPosition;

        public Vector2 TargetPosition
        {
            get
            {
                return targetPosition;
            }

            set
            {
                targetPosition = value;

                if(onTargetChanged != null)
                {
                    onTargetChanged(targetPosition);
                }
            }
        }
    }

    private Blackboard blackboard;

    private FSM<State, Trigger> alertFSM;
    private FSM<State, Trigger> searchTargetFSM;

    [SerializeField]
    private TurretAlertlessState turretAlertlessState;

    [SerializeField]
    private TurretAlertFulState turretAlertFulState;

    [SerializeField]
    private TurretWithoutTargetState turretWithoutTargetState;

    [SerializeField]
    private TurretWithTargetState turretWithTargetState;

    private Eyes characterEyes;

    public Eyes CharacterEyes
    {
        get
        {
            if(Slave != null)
            {
                if(characterEyes == null)
                {
                    characterEyes = Slave.GetComponentInChildren<Eyes>();
                }
            }
            else
            {
                characterEyes = null;
            }

            return characterEyes;
        }
    }

    void Awake()
    {
        blackboard = new Blackboard();

        alertFSM = new FSM<State, Trigger>();
        searchTargetFSM = new FSM<State, Trigger>();

        turretAlertlessState.InitializeState(alertFSM, State.Alertless, this, blackboard);
        turretAlertFulState.InitializeState(alertFSM, State.Alertful, this, blackboard);

        alertFSM.AddState(turretAlertlessState);
        alertFSM.AddState(turretAlertFulState);

        alertFSM.MakeTransition(State.Alertless, Trigger.SetFullAlert, State.Alertful);
        alertFSM.MakeTransition(State.Alertful, Trigger.CalmDown, State.Alertless);

        alertFSM.StartBy(State.Alertless);

        turretWithoutTargetState.InitializeState(searchTargetFSM, State.WithoutTarget, this, blackboard);
        turretWithTargetState.InitializeState(searchTargetFSM, State.WithTarget, this, blackboard);

        searchTargetFSM.AddState(turretWithoutTargetState);
        searchTargetFSM.AddState(turretWithTargetState);

        searchTargetFSM.MakeTransition(State.WithoutTarget, Trigger.TargetFound, State.WithTarget);
        searchTargetFSM.MakeTransition(State.WithTarget, Trigger.TargetLost, State.WithoutTarget);
        
        searchTargetFSM.StartBy(State.WithoutTarget);


    }

    void Update()
    {
        Control();
    }

    public override void Control()
    {
        alertFSM.UpdateCurrentState();
        searchTargetFSM.UpdateCurrentState();
    }
}
