using SAM.FSM;
using UnityEngine;
using System;

public class XenophobicIAController : UnityController<Xenophobic, Character.Trigger>
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

    public Eyes SlaveEyes { get; protected set; }

    /*protected Blackboard blackboard;

    [SerializeField]
    protected XenophobicAlertlessState alertlessState;

    [SerializeField]
    protected XenophobicAwareState awareState;

    [SerializeField]
    protected XenophobicAlertfulState alertfulState;

    [SerializeField]
    protected XenophobicPatrolState patrolState;

    [SerializeField]
    protected XenophobicSeekState seekState;

    protected FSM<State, Trigger> alertnessFSM;
    protected FSM<State, Trigger> behaviourFSM;

    



    protected override void Start()
    {
        base.Start();

        blackboard = new Blackboard();
        SlaveEyes = Slave.GetComponentInChildren<Eyes>();

        alertnessFSM = new FSM<State, Trigger>();

        alertlessState.InitializeState(alertnessFSM, State.Alertless, this, blackboard);
        awareState.InitializeState(alertnessFSM, State.Aware, this, blackboard);
        alertfulState.InitializeState(alertnessFSM, State.Alertful, this, blackboard);

        alertnessFSM.AddState(alertlessState);
        alertnessFSM.AddState(awareState);
        alertnessFSM.AddState(alertfulState);

        alertnessFSM.MakeTransition(State.Alertless, Trigger.GetAware, State.Aware);
        alertnessFSM.MakeTransition(State.Aware, Trigger.SetFullAlert, State.Alertful);
        alertnessFSM.MakeTransition(State.Alertful, Trigger.CalmDown, State.Aware);
        alertnessFSM.MakeTransition(State.Aware, Trigger.CalmDown, State.Alertless);

        alertnessFSM.StartBy(State.Alertless);


        behaviourFSM = new FSM<State, Trigger>();

        patrolState.InitializeState(behaviourFSM, State.Patrolling, this, blackboard);
        seekState.InitializeState(behaviourFSM, State.Seeking, this, blackboard);

        behaviourFSM.AddState(patrolState);
        behaviourFSM.AddState(seekState);

        behaviourFSM.MakeTransition(State.Patrolling, Trigger.Seek, State.Seeking);
        behaviourFSM.MakeTransition(State.Seeking, Trigger.Patrol, State.Patrolling);

        behaviourFSM.StartBy(State.Patrolling);
    }*/

    public override void Control()
    {
        /*alertnessFSM.UpdateCurrentState();
        behaviourFSM.UpdateCurrentState();*/
    }

    protected void Update()
    {
        if(Slave != null)
        {
            Control();
        }
        
    }

    public override bool ShouldBeSaved()
    {
        return true;
    }

    private Guid slaveGuid;

    protected override void OnSave(SaveData data)
    {
        data.AddValue("s", Slave.saveGUID);
    }

    protected override void OnLoad(SaveData data)
    {
        slaveGuid = new Guid(data.GetAs<string>("s"));
    }

    public override void PostLoadCallback(SaveData data)
    {
        Xenophobic slave = SJMonoBehaviourSaveable.GetSJMonobehaviourSaveableBySaveGUID<Xenophobic>(slaveGuid);

        if(slave != null)
        {
            SetSlave(slave);
        }
    }
}
