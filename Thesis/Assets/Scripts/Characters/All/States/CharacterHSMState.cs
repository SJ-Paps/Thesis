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

    protected override void OnEnter()
    {
#if UNITY_EDITOR
        if(activeDebug)
        {
            EditorDebug.Log(DebugName + " ENTER " + Owner.name);
        }
#endif
    }

    protected void Log(object obj)
    {
#if UNITY_EDITOR
        if(activeDebug)
        {
            UnityEngine.Debug.Log(obj);
        }
#endif
    }


    protected override void OnUpdate()
    {

    }

    protected override void OnExit()
    {
#if UNITY_EDITOR
        if(activeDebug)
        {
            EditorDebug.Log(DebugName + " EXIT " + Owner.name);
        }
#endif
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
