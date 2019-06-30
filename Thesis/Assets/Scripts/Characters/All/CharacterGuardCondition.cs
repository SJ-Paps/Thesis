public abstract class CharacterGuardCondition : SJGuardCondition
{
    protected Blackboard Blackboard { get; private set; }
    protected new CharacterConfiguration Configuration { get; private set; }

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        Configuration = (CharacterConfiguration)base.Configuration;
        Blackboard = Configuration.Blackboard;
    }
}
