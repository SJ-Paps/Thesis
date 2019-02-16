using System.Collections;
using UnityEngine;

public class TribalClimbingLedgeState : TribalClimbingState
{
    private IEnumerator climbCoroutine;

    public TribalClimbingLedgeState(Character.State stateId, string debugName = null) : base(stateId, debugName)
    {
        activeDebug = true;

        climbCoroutine = ClimbCoroutine();
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        character.RigidBody2D.velocity = new Vector2(0, 0);
        character.RigidBody2D.isKinematic = true;

        CoroutineManager.GetInstance().StartCoroutine(climbCoroutine);
    }

    protected override void OnExit()
    {
        character.RigidBody2D.isKinematic = false;

        CoroutineManager.GetInstance().StopCoroutine(climbCoroutine);
    }

    private IEnumerator ClimbCoroutine()
    {
        while(true)
        {
            float endPointOffsetY = 0.02f;

            Vector2 startPoint = character.transform.position;

            Vector2 endPointForY = new Vector2(character.transform.position.x, blackboard.ledgeCheckHit.point.y + character.Collider.bounds.extents.y + endPointOffsetY);
            Vector2 endPointForX = new Vector2(blackboard.ledgeCheckHit.point.x, endPointForY.y);

            float timeAcummulator = 0;
            float time = 0.5f;

            while (timeAcummulator < time)
            {
                timeAcummulator += Time.deltaTime;

                character.transform.position = Vector3.Lerp(startPoint, endPointForY, timeAcummulator / time);
                yield return null;
            }

            timeAcummulator = 0;

            while (timeAcummulator < time)
            {
                timeAcummulator += Time.deltaTime;

                character.transform.position = Vector3.Lerp(endPointForY, endPointForX, timeAcummulator / time);
                yield return null;
            }

            character.transform.position = endPointForX;

            SendEvent(Character.Trigger.StopHanging);

            CoroutineManager.GetInstance().StopCoroutine(climbCoroutine);

            yield return null;
        }
    }
}