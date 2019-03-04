public class XenophobicIAAlertlessState : XenophobicIAState
{
    private float previousHiddenDetectionDistance;
    private float previousHiddenDetectionProbability;

    public XenophobicIAAlertlessState(byte stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        previousHiddenDetectionDistance = Owner.CurrentHiddenDetectionDistance;
        previousHiddenDetectionProbability = Owner.CurrentHiddenDetectionProbability;

        Owner.CurrentHiddenDetectionDistance = Owner.Configuration.HiddenDetectionDistanceAlertless;
        Owner.CurrentHiddenDetectionProbability = Owner.Configuration.HiddenDetectionProbabilityAlertless;
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.CurrentHiddenDetectionDistance = previousHiddenDetectionDistance;
        Owner.CurrentHiddenDetectionProbability = previousHiddenDetectionProbability;
    }
}
