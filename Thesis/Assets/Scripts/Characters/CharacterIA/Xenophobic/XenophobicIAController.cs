using SAM.FSM;
using UnityEngine;
using System;

public class XenophobicIAController : UnityController<Xenophobic, Character.Order>
{

    public enum State
    {
        Alertless,
        Aware,
        Alertful,
        Patrolling,
        Seeking
    }

    public enum Trigger
    {
        CalmDown,
        GetAware,
        SetFullAlert,
        Patrol,
        Seek
    }

    public class Blackboard
    {
        public event Action<Vector2> onLastDetectionPositionChanged;
        public event Action<Character> onPlayerDataChanged;

        private Character playerData;

        public Character PlayerData
        {
            get
            {
                return playerData;
            }

            set
            {
                playerData = value;

                if(onPlayerDataChanged != null)
                {
                    onPlayerDataChanged(playerData);
                }
            }
        }

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

        alertnessFSM = new FSM<State, Trigger>();

        alertnessFSM.AddState(new XenophobicAlertlessState(alertnessFSM, State.Alertless, this, blackboard));
        alertnessFSM.AddState(new XenophobicAwareState(alertnessFSM, State.Aware, this, blackboard));
        alertnessFSM.AddState(new XenophobicFullAlertState(alertnessFSM, State.Alertful, this, blackboard));

        alertnessFSM.MakeTransition(State.Alertless, Trigger.GetAware, State.Aware);
        alertnessFSM.MakeTransition(State.Aware, Trigger.SetFullAlert, State.Alertful);
        alertnessFSM.MakeTransition(State.Alertful, Trigger.CalmDown, State.Aware);
        alertnessFSM.MakeTransition(State.Aware, Trigger.CalmDown, State.Alertless);

        alertnessFSM.StartBy(State.Alertless);


        behaviourFSM = new FSM<State, Trigger>();

        behaviourFSM.AddState(new XenophobicPatrol(behaviourFSM, State.Patrolling, this, blackboard));
        behaviourFSM.AddState(new XenophobicSeek(behaviourFSM, State.Seeking, this, blackboard));

        behaviourFSM.MakeTransition(State.Patrolling, Trigger.Seek, State.Seeking);
        behaviourFSM.MakeTransition(State.Seeking, Trigger.Patrol, State.Patrolling);

        behaviourFSM.StartBy(State.Patrolling);

        //Super provisorio
        Slave.Weapon.SetUser(Slave);
    }

    public override void Control()
    {
        alertnessFSM.UpdateCurrentState();
        behaviourFSM.UpdateCurrentState();
    }

    protected void Update()
    {
        if(Slave != null)
        {
            Control();
        }
        
    }


}
