using UnityEngine;

public class TribalGrapplingLadderState : TribalHSMState
{
    public TribalGrapplingLadderState(byte stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.RigidBody2D.velocity = new Vector2(0, 0);
    }

    
}
