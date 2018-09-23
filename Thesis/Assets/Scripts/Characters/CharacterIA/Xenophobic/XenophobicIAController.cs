using SAM.FSM;
using UnityEngine;
using System;

public class XenophobicIAController : UnityController<Xenophobic, Character.Order>
{
    public enum AlertState
    {
        CalmedDown,
        Aware,
        FullAlert
    }

    public enum AlertTrigger
    {
        CalmDown,
        GetAware,
        SetFullAlert
    }

    public enum CharacterBehaviour
    {
        Patrol,
        Seek,
        Chase
    }

    public enum CharacterBehaviourTrigger
    {
        SomethingDetected,
        PlayerDetected
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

    protected FSM<AlertState, AlertTrigger> alertnessFSM;

    public Eyes SlaveEyes { get; protected set; }

    void Start()
    {
        blackboard = new Blackboard();
        SlaveEyes = Slave.GetComponentInChildren<Eyes>();
        SlaveEyes.SetEyePoint(Slave.EyePoint);

        alertnessFSM = new FSM<AlertState, AlertTrigger>();
        

        alertnessFSM.AddState(new XenophobicAlertlessState(alertnessFSM, AlertState.CalmedDown, this, blackboard));
        alertnessFSM.AddState(new XenophobicAwareState(alertnessFSM, AlertState.Aware, this, blackboard));
        alertnessFSM.AddState(AlertState.FullAlert);

        alertnessFSM.MakeTransition(AlertState.CalmedDown, AlertTrigger.GetAware, AlertState.Aware);
        alertnessFSM.MakeTransition(AlertState.Aware, AlertTrigger.SetFullAlert, AlertState.FullAlert);
        alertnessFSM.MakeTransition(AlertState.FullAlert, AlertTrigger.CalmDown, AlertState.Aware);
        alertnessFSM.MakeTransition(AlertState.Aware, AlertTrigger.CalmDown, AlertState.CalmedDown);

        alertnessFSM.StartBy(AlertState.CalmedDown);
    }

    public override void Control()
    {
        alertnessFSM.UpdateCurrentState();
    }

    void Update()
    {
        Control();
    }

}
