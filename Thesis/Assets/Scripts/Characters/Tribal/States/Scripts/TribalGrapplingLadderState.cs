using UnityEngine;

public class TribalGrapplingLadderState : TribalHSMState
{

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.RigidBody2D.velocity = new Vector2(0, 0);
    }

    
}
