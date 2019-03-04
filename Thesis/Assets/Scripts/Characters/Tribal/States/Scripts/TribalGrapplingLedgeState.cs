using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalGrapplingLedgeState : TribalGrapplingState
{
    private bool isFirstUpdate;

    private SpringJoint2D grapplingPoint;

    public TribalGrapplingLedgeState(byte stateId, string debugName = null) : base(stateId, debugName)
    {
        grapplingPoint = new GameObject("Character Grappling Ledge Point").AddComponent<SpringJoint2D>();
        grapplingPoint.autoConfigureDistance = false;
        grapplingPoint.distance = 0.2f;
        grapplingPoint.dampingRatio = 1;
        grapplingPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        grapplingPoint.gameObject.SetActive(false);
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        int xDirection;

        if (Owner.transform.right.x >= 0)
        {
            xDirection = 1;
        }
        else
        {
            xDirection = -1;
        }

        grapplingPoint.gameObject.SetActive(true);
        grapplingPoint.transform.position = new Vector2(Owner.Collider.bounds.center.x + (xDirection * Owner.Collider.bounds.extents.x), Blackboard.ledgeCheckHit.point.y);

        isFirstUpdate = true;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(isFirstUpdate)
        {
            OnFirstUpdate();
            isFirstUpdate = false;
        }
    }

    protected override void OnExit()
    {
        base.OnExit();

        Release();
    }

    private void OnFirstUpdate()
    {
        Grapple();
    }

    private void Grapple()
    {
        grapplingPoint.connectedBody = Owner.RigidBody2D;
        
    }

    private void Release()
    {
        grapplingPoint.connectedBody = null;
        grapplingPoint.gameObject.SetActive(false);
        
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if(trigger == Character.Order.ClimbDown)
        {
            SendEvent(Character.Order.StopHanging);
            return true;
        }
        else if(trigger == Character.Order.ClimbUp || trigger == Character.Order.Jump)
        {
            int xDirection = 0;

            if (Owner.transform.right.x >= 0)
            {
                xDirection = 1;
            }
            else
            {
                xDirection = -1;
            }

            float yCenterOffset = 0.01f;

            if (IsValidForGrapplingAndClimbing(new Vector2(grapplingPoint.transform.position.x + (xDirection * Owner.Collider.bounds.extents.x),
                                                grapplingPoint.transform.position.y + Owner.Collider.bounds.extents.y + yCenterOffset),
                                                Owner.Collider.bounds.size))
            {
                return false; //dejo que transicione
            }
            else
            {
                return true; //no lo dejo transicionar
            }
        }

        return false;
    }

    private bool IsValidForGrapplingAndClimbing(Vector2 center, Vector2 boxSize)
    {
        Collider2D possibleObstacle = Physics2D.OverlapCapsule(center, boxSize, CapsuleDirection2D.Vertical, Reg.walkableLayerMask);

        return possibleObstacle == null;
    }
}