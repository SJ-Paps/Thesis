using System.Collections;
using UnityEngine;
using SJ.Coroutines;

public class TribalClimbingLedgeState : TribalHSMState
{
    private IEnumerator climbCoroutine;
    private ICoroutineScheduler coroutineScheduler;
    

    public TribalClimbingLedgeState()
    {
        coroutineScheduler = SJ.Application.GetCoroutineScheduler();

        climbCoroutine = ClimbCoroutine();
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Owner.RigidBody2D.velocity = new Vector2(0, 0);
        Owner.RigidBody2D.isKinematic = true;

        coroutineScheduler.StartCoroutine(climbCoroutine);
    }

    protected override void OnExit()
    {
        Owner.RigidBody2D.isKinematic = false;

        coroutineScheduler.StopCoroutine(climbCoroutine);
    }

    private IEnumerator ClimbCoroutine()
    {
        while(true)
        {
            float endPointOffsetY = 0.02f;

            Vector2 startPoint = Owner.transform.position;

            RaycastHit2D ledgeCheckHit = Blackboard.GetItemOf<RaycastHit2D>("LedgeCheckHit");

            Vector2 endPointForY = new Vector2(Owner.transform.position.x, ledgeCheckHit.point.y + Owner.Collider.bounds.extents.y + endPointOffsetY);
            Vector2 endPointForX = new Vector2(ledgeCheckHit.point.x, endPointForY.y);

            float timeAcummulator = 0;
            float time = 0.5f;

            while (timeAcummulator < time)
            {
                timeAcummulator += Time.deltaTime;

                Owner.transform.position = Vector3.Lerp(startPoint, endPointForY, timeAcummulator / time);
                yield return null;
            }

            timeAcummulator = 0;

            while (timeAcummulator < time)
            {
                timeAcummulator += Time.deltaTime;

                Owner.transform.position = Vector3.Lerp(endPointForY, endPointForX, timeAcummulator / time);
                yield return null;
            }

            Owner.transform.position = endPointForX;

            SendEvent(Character.Order.FinishAction);

            coroutineScheduler.StopCoroutine(climbCoroutine);

            yield return null;
        }
    }

    protected override bool HandleEvent(Character.Order trigger)
    {
        if(trigger == Character.Order.Jump)
        {
            return true;
        }

        return false;
    }
}