using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribalGrapplingLedgeState : TribalGrapplingState
{
    private bool isFirstUpdate;

    private SpringJoint2D grapplingPoint;

    public TribalGrapplingLedgeState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        activeDebug = true;

        grapplingPoint = new GameObject("Character Grappling Ledge Point").AddComponent<SpringJoint2D>();
        grapplingPoint.autoConfigureDistance = false;
        grapplingPoint.distance = 0.2f;
        grapplingPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        grapplingPoint.gameObject.SetActive(false);
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        
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
        int xDirection;

        if(character.transform.right.x >= 0)
        {
            xDirection = 1;
        }
        else
        {
            xDirection = -1;
        }

        grapplingPoint.gameObject.SetActive(true);
        grapplingPoint.transform.position = new Vector2(character.Collider.bounds.center.x + (xDirection * character.Collider.bounds.extents.x), blackboard.ledgeCheckHit.point.y);
        grapplingPoint.connectedBody = character.RigidBody2D;
        
    }

    private void Release()
    {
        grapplingPoint.connectedBody = null;
        grapplingPoint.gameObject.SetActive(false);
        
    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        if(trigger == Character.Trigger.ClimbDown)
        {
            SendEvent(Character.Trigger.StopHanging);
            return true;
        }

        return false;
    }
}