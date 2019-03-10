using UnityEngine;

public class TribalGrapplingWallState : TribalHSMState
{
    public TribalGrapplingWallState(byte stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.RigidBody2D.velocity = new Vector2(0, 0);
    }


}
