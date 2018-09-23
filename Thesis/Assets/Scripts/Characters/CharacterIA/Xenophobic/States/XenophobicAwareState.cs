using SAM.FSM;
using SAM.Timers;
using System;
using UnityEngine;

public class XenophobicAwareState : XenophobicIAState
{
    private Eyes characterEyes;

    private SyncTimer awareTimer;
    private float awareTime = 4f;

    private Vector2 eyesSize = new Vector2(10, 10);

    private float fullAlertDetectionDistance = 6f;

    private Action<Collider2D> onSomethingDetectedStayDelegate;

    public XenophobicAwareState(FSM<XenophobicIAController.AlertState, XenophobicIAController.AlertTrigger> fsm, XenophobicIAController.AlertState state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state, controller, blackboard)
    {
        characterEyes = controller.SlaveEyes;

        awareTimer = new SyncTimer();
        awareTimer.Interval = awareTime;
        awareTimer.onTick += CalmDown;

        onSomethingDetectedStayDelegate += AnalyzeDetection;
    }

    protected override void OnEnter()
    {
        if (characterEyes != null)
        {
            awareTimer.Start();

            characterEyes.Trigger2D.onStay += onSomethingDetectedStayDelegate;
        }
    }

    protected override void OnUpdate()
    {
        awareTimer.Update(Time.deltaTime);
    }

    protected override void OnExit()
    {
        if (characterEyes != null)
        {
            awareTimer.Stop();

            characterEyes.Trigger2D.onStay -= onSomethingDetectedStayDelegate;
        }
    }

    /*private void SearchAtPosition(Vector2 position)
    {
        if (position.x < controller.Slave.transform.position.x)
        {
            controller.Slave.SetOrder(Character.Order.OrderMoveLeft);
        }
        else
        {
            controller.Slave.SetOrder(Character.Order.OrderMoveRight);
        }
    }

    private bool IsPositionReached(Vector2 position)
    {
        Bounds b = new Bounds(position, new Vector2(positionReachedMarginX * 2, positionReachedMarginY * 2));

        if (b.Contains(controller.Slave.transform.position))
        {
            return true;
        }

        return false;
    }*/

    private void UpdatePosition(Vector2 position)
    {
        blackboard.LastDetectionPosition = position;
        awareTimer.Start();
    }

    private void AnalyzeDetection(Collider2D collider)
    {
        UpdatePosition(collider.transform.position);

        if (Vector2.Distance(collider.transform.position, controller.Slave.transform.position) <= fullAlertDetectionDistance)
        {
            SetFullAlert();
        }
    }

    private void SetFullAlert()
    {
        stateMachine.Trigger(XenophobicIAController.AlertTrigger.SetFullAlert);
    }

    private void CalmDown(SyncTimer timer)
    {
        CalmDown();
    }

    private void CalmDown()
    {
        stateMachine.Trigger(XenophobicIAController.AlertTrigger.CalmDown);
    }
}
