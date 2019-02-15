using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SAM.Timers;

public class TribalClimbingLedgeState : TribalClimbingState
{
    private RigidbodyType2D previousRigidbody2DType;

    private IEnumerator climbCoroutine;

    public TribalClimbingLedgeState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        activeDebug = true;

        climbCoroutine = ClimbCoroutine();
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        previousRigidbody2DType = character.RigidBody2D.bodyType;
        
        character.RigidBody2D.isKinematic = true;
        character.Collider.IsTrigger = true;

        CoroutineManager.GetInstance().StartCoroutine(climbCoroutine);
    }

    protected override void OnExit()
    {
        character.RigidBody2D.isKinematic = false;
        character.Collider.IsTrigger = false;

        CoroutineManager.GetInstance().StopCoroutine(climbCoroutine);
    }

    private IEnumerator ClimbCoroutine()
    {
        while(true)
        {
            float endPointOffsetX = 0.2f;
            float endPointOffsetY = 0.1f;

            float xDirection;

            if(character.transform.right.x >= 0)
            {
                xDirection = 1;
            }
            else
            {
                xDirection = -1;
            }

            Vector2 startPoint = character.transform.position;
            Vector2 endPoint = new Vector2(blackboard.ledgeCheckHit.point.x + (endPointOffsetX * xDirection), blackboard.ledgeCheckHit.point.y + character.Collider.bounds.extents.y + endPointOffsetY);

            float acum = 0;
            float time = 0.5f;

            while (acum < time)
            {
                acum += Time.deltaTime;

                character.transform.position = Vector3.Lerp(startPoint, endPoint, acum / time);
                yield return null;
            }

            character.transform.position = endPoint;

            SendEvent(Character.Trigger.StopHanging);

            CoroutineManager.GetInstance().StopCoroutine(climbCoroutine);

            yield return null;
        }
    }
}