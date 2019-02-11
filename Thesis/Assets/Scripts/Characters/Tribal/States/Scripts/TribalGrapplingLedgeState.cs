using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalGrapplingLedgeState : TribalGrapplingState
{
    private float previousGravityScale;

    public TribalGrapplingLedgeState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        activeDebug = true;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        previousGravityScale = character.RigidBody2D.gravityScale;

        character.RigidBody2D.gravityScale = 0;
        character.RigidBody2D.velocity = new Vector2(0, 0);
    }

    protected override void OnExit()
    {
        base.OnExit();

        character.RigidBody2D.gravityScale = previousGravityScale;
    }
}