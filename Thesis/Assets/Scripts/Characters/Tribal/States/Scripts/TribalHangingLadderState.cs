﻿using UnityEngine;

public class TribalHangingLadderState : TribalHSMState
{
    private float previousGravityScale;

    public TribalHangingLadderState(byte stateId, string debugName = null) : base(stateId, debugName)
    {

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Ladder ladder = (Ladder)Blackboard.activable;
        
        Owner.RigidBody2D.velocity = new Vector2(0, 0);

        previousGravityScale = Owner.RigidBody2D.gravityScale;
        Owner.RigidBody2D.gravityScale = 0;
    }

    protected override void OnExit()
    {
        base.OnExit();

        Owner.RigidBody2D.gravityScale = previousGravityScale;
    }
}
