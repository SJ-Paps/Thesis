using SAM.FSM;
using UnityEngine;

public class XenophobicIAController : UnityController<Xenophobic, Character.Order>
{
    public enum State
    {
        CalmedDown,
        Aware,
        FullAlert
    }

    public enum Trigger
    {
        CalmDown,
        GetAware,
        SetFullAlert
    }

    public class Blackboard
    {
        public Vector2 seekedLastPosition;
    }

    protected Blackboard blackboard;

    protected FSM<State, Trigger> alertnessFSM;

    void Awake()
    {
        blackboard = new Blackboard();

        alertnessFSM = new FSM<State, Trigger>();

        alertnessFSM.AddState(new XenophobicAlertlessState(alertnessFSM, State.CalmedDown, slave, blackboard));
        alertnessFSM.AddState(new XenophobicAwareState(alertnessFSM, State.Aware, slave, blackboard));
        alertnessFSM.AddState(State.FullAlert);

        alertnessFSM.MakeTransition(State.CalmedDown, Trigger.GetAware, State.Aware);
        alertnessFSM.MakeTransition(State.Aware, Trigger.SetFullAlert, State.FullAlert);
        alertnessFSM.MakeTransition(State.FullAlert, Trigger.CalmDown, State.Aware);
        alertnessFSM.MakeTransition(State.Aware, Trigger.CalmDown, State.CalmedDown);

        alertnessFSM.StartBy(State.CalmedDown);
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
