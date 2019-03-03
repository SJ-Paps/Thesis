using UnityEngine;

public abstract class CharacterHSMState<TState> : SJHSMState<TState, Character.Order> where TState : unmanaged
{
    protected Character.Order LastEnteringTrigger { get; private set; }

    protected CharacterHSMState(TState state, string debugName = null) : base(state, debugName)
    {
        
    }

    protected override void OnOwnerReferencePropagated()
    {
        onAnyStateChanged += CatchEnteringTrigger;
    }

    private void CatchEnteringTrigger(Character.Order trigger)
    {
        if(IsOnState(StateId))
        {
            LastEnteringTrigger = trigger;

            for(int i = 0; i < parallelChilds.Count; i++)
            {
                ((CharacterHSMState<TState>)parallelChilds[i]).LastEnteringTrigger = trigger;
            }

            if(ActiveNonParallelChild != null)
            {
                ((CharacterHSMState<TState>)ActiveNonParallelChild).LastEnteringTrigger = trigger;
            }
        }
    }
}
