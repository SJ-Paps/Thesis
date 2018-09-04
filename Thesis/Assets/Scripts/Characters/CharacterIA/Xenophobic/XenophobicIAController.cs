using SAM.FSM;

public class XenophobicIAController : UnityController<Xenophobic, Character.Order>
{
    public enum Alertness
    {
        CalmedDown,
        Aware,
        FullAlert
    }

    public enum AlertTrigger
    {
        GoNext,
        GoPrevious
    }

    protected FSM<Alertness, AlertTrigger> alertnessFSM;

    void Awake()
    {
        alertnessFSM = new FSM<Alertness, AlertTrigger>();

        alertnessFSM.AddState(Alertness.CalmedDown);
        alertnessFSM.AddState(Alertness.Aware);
        alertnessFSM.AddState(Alertness.FullAlert);

        alertnessFSM.MakeTransition(Alertness.CalmedDown, AlertTrigger.GoNext, Alertness.Aware);
        alertnessFSM.MakeTransition(Alertness.Aware, AlertTrigger.GoNext, Alertness.FullAlert);
        alertnessFSM.MakeTransition(Alertness.FullAlert, AlertTrigger.GoPrevious, Alertness.Aware);
        alertnessFSM.MakeTransition(Alertness.Aware, AlertTrigger.GoPrevious, Alertness.CalmedDown);

        alertnessFSM.StartBy(Alertness.CalmedDown);
    }

    public override void Control()
    {
        
    }

    void Update()
    {
        Control();
    }


}
