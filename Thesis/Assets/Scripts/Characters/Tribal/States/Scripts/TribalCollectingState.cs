public class TribalCollectingState : TribalHSMState
{
    public TribalCollectingState(byte stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        CollectableObject collectableObject = Blackboard.activable as CollectableObject;

        Blackboard.activable = null;

        if (collectableObject != null)
        {
            Owner.GetHand().CollectObject(collectableObject);
        }

        SendEvent(Character.Order.FinishAction);
    }
}
