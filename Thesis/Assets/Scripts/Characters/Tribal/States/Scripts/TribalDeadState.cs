public class TribalDeadState : TribalHSMState
{
    public TribalDeadState(Character.State state, string debugName) : base(state, debugName)
    {
        activeDebug = true;
    }
}
