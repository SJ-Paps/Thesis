public abstract class CharacterHSMState : SJHSMState<Character.State, Character.Trigger, Character, Character.Blackboard>
{
    protected Character.Trigger LastEnteringTrigger { get; private set; }

    protected CharacterHSMState(Character.State state, string debugName = null) : base(state, debugName)
    {

    }

    protected override void OnOwnerReferencePropagated()
    {
        onAnyStateChanged += CatchEnteringTrigger;
    }

    

    private void CatchEnteringTrigger(Character.Trigger trigger)
    {
        if(IsOnState(StateId))
        {
            LastEnteringTrigger = trigger;

            for(int i = 0; i < parallelChilds.Count; i++)
            {
                ((CharacterHSMState)parallelChilds[i]).LastEnteringTrigger = trigger;
            }

            if(ActiveNonParallelChild != null)
            {
                ((CharacterHSMState)ActiveNonParallelChild).LastEnteringTrigger = trigger;
            }
        }
    }
}
