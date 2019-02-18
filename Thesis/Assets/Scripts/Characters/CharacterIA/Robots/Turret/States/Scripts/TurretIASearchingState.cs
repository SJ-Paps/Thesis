using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretIASearchingState : TurretIAControllerHSMState
{
    private HingeJoint2D joint;

    private float angleLimitDeadZone = 5f;

    private Character.Trigger currentOrder;

    public TurretIASearchingState(TurretIAController.State stateId, string debugName) : base(stateId, debugName)
    {
        currentOrder = Character.Trigger.MoveRight;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        float currentRotation = joint.jointAngle;

        if(currentRotation < joint.limits.max + angleLimitDeadZone)
        {
            currentOrder = Character.Trigger.MoveRight;
        }
        else if(currentRotation > joint.limits.min - angleLimitDeadZone)
        {
            currentOrder = Character.Trigger.MoveLeft;
        }

        Owner.Slave.SetOrder(currentOrder);
    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        joint = Owner.Slave.GetComponentInChildren<HingeJoint2D>();
    }
}
