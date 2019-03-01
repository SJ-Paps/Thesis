public class TribalDeadState : TribalHSMState
{
    public TribalDeadState(Tribal.State state, string debugName) : base(state, debugName)
    {
        activeDebug = true;
    }
}
