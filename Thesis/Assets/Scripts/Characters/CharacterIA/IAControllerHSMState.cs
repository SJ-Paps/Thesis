public class IAControllerHSMState : SJHSMState
{
    protected new IAControllerConfiguration Configuration { get; private set; }
    protected Blackboard Blackboard { get; private set; }

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        Configuration = (IAControllerConfiguration)base.Configuration;
        Blackboard = Configuration.Blackboard;
    }
}
