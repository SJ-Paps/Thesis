public class TribalDeadState : TribalHSMState
{
    public TribalDeadState(byte stateId, string debugName) : base(stateId, debugName)
    {
        activeDebug = true;
    }
}
