using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretIASearchingState : TurretIAControllerHSMState
{
    private HingeJoint2D joint;

    private float angleLimitDeadZone = 5f;

    private Character.Order currentOrder;

    public TurretIASearchingState(byte stateId, string debugName = null) : base(stateId, debugName)
    {
        currentOrder = Character.Order.MoveRight;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        float currentRotation = joint.jointAngle;

        if(currentRotation < joint.limits.max + angleLimitDeadZone)
        {
            currentOrder = Character.Order.MoveRight;
        }
        else if(currentRotation > joint.limits.min - angleLimitDeadZone)
        {
            currentOrder = Character.Order.MoveLeft;
        }

        Owner.Slave.SendOrder(currentOrder);
    }

    protected override void OnOwnerReferencePropagated()
    {
        base.OnOwnerReferencePropagated();

        joint = Owner.Slave.GetComponentInChildren<HingeJoint2D>();
    }
}
