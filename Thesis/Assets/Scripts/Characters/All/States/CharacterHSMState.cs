using UnityEngine;
using Paps.StateMachines.HSM;

public abstract class CharacterHSMState : SJHSMState
{
    protected Character.Order LastEnteringTrigger { get; private set; }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        onAnyStateChanged += CatchEnteringTrigger;
    }

    private void CatchEnteringTrigger(HSMState<byte, byte> stateFrom, byte trigger, HSMState<byte, byte> stateTo)
    {
        if(IsOnState(this, stateTo))
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
