public class XenophobicIAAlertlessState : XenophobicIAState
{
    private float previousHiddenDetectionDistance;
    private float previousHiddenDetectionProbability;

    protected override void OnEnter()
    {
        base.OnEnter();

        previousHiddenDetectionDistance = Owner.CurrentHiddenDetectionDistance;
        previousHiddenDetectionProbability = Owner.CurrentHiddenDetectionProbability;

        Owner.CurrentHiddenDetectionDistance = Owner.HiddenDetectionDistanceAlertless;
        Owner.CurrentHiddenDetectionProbability = Owner.HiddenDetectionProbabilityAlertless;
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.CurrentHiddenDetectionDistance = previousHiddenDetectionDistance;
        Owner.CurrentHiddenDetectionProbability = previousHiddenDetectionProbability;
    }
}
