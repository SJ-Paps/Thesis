using UnityEngine;

public class TribalGrapplingWallState : TribalHSMState
{
    protected override void OnEnter()
    {
        base.OnEnter();

        Configuration.RigidBody2D.velocity = new Vector2(0, 0);
    }


}
