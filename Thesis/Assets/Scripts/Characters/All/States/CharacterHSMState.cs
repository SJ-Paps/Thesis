using UnityEngine;

public abstract class CharacterHSMState : SJHSMState
{
    protected Character.Order LastEnteringTrigger { get; private set; }

    protected CharacterHSMState(byte state, string debugName = null) : base(state, debugName)
    {
        
    }

    protected override void OnOwnerReferencePropagated()
    {
        onAnyStateChanged += CatchEnteringTrigger;
    }

    private void CatchEnteringTrigger(byte trigger)
    {
        if(IsOnState(StateId))
        {
            LastEnteringTrigger = (Character.Order)trigger;

            for(int i = 0; i < parallelChilds.Count; i++)
            {
                ((CharacterHSMState)parallelChilds[i]).LastEnteringTrigger = LastEnteringTrigger;
            }

            if(ActiveNonParallelChild != null)
            {
                ((CharacterHSMState)ActiveNonParallelChild).LastEnteringTrigger = LastEnteringTrigger;
            }
        }
    }

    public bool SendEvent(Character.Order trigger)
    {
        return SendEvent((byte)trigger);
    }

    protected sealed override bool HandleEvent(byte trigger)
    {
        return HandleEvent((Character.Order)trigger);
    }

    protected virtual bool HandleEvent(Character.Order trigger)
    {
        return false;
    }
}
