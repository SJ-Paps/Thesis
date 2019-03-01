using UnityEngine;

public abstract class CharacterHSMState<TState, TCharacter, TBlackboard> : SJHSMState<TState, Character.Trigger, TCharacter, TBlackboard> where TState : unmanaged where TCharacter : class
{
    protected Character.Trigger LastEnteringTrigger { get; private set; }

    protected CharacterHSMState(TState state, string debugName = null) : base(state, debugName)
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
                ((CharacterHSMState<TState, TCharacter, TBlackboard>)parallelChilds[i]).LastEnteringTrigger = trigger;
            }

            if(ActiveNonParallelChild != null)
            {
                ((CharacterHSMState<TState, TCharacter, TBlackboard>)ActiveNonParallelChild).LastEnteringTrigger = trigger;
            }
        }
    }
}
