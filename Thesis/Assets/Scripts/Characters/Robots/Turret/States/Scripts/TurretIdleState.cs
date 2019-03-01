using System;

public class TurretIdleState : TurretHSMState
{
    public TurretIdleState(Turret.State state, string debugName) : base(state, debugName)
    {
    }
}
