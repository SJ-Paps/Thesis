using SAM.FSM;
using SAM.Timers;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class XenophobicFullAlertState : XenophobicIAState
{
    private Vector2 eyeSize = new Vector2(18, 5);

    private Eyes characterEyes;

    private SyncTimer fullAlertTimer;
    private float fullAlertTime = 8f;

    private Action<Collider2D> onSomethingDetectedDelegate;

    private int visionLayers = (1 << Reg.floorLayer) | (1 << Reg.playerLayer) | (1 << Reg.objectLayer);

    private float findProbability = 75;

    public XenophobicFullAlertState(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state, controller, blackboard)
    {
        characterEyes = controller.SlaveEyes;

        fullAlertTimer = new SyncTimer();
        fullAlertTimer.Interval = fullAlertTime;
        fullAlertTimer.onTick += CalmDown;

        onSomethingDetectedDelegate += AnalyzeDetection;
    }

    protected override void OnEnter()
    {
        if(characterEyes != null)
        {
            characterEyes.Trigger2D.ChangeSize(eyeSize);

            characterEyes.Trigger2D.onStay += onSomethingDetectedDelegate;

            fullAlertTimer.Start();
        }
    }

    protected override void OnUpdate()
    {
        fullAlertTimer.Update(Time.deltaTime);
    }

    protected override void OnExit()
    {
        if(characterEyes != null)
        {
            characterEyes.Trigger2D.onStay -= onSomethingDetectedDelegate;

            fullAlertTimer.Stop();
        }
    }

    private void CalmDown(SyncTimer timer)
    {
        CalmDown();
    }

    private void CalmDown()
    {
        stateMachine.Trigger(XenophobicIAController.Trigger.CalmDown);
    }

    private void AnalyzeDetection(Collider2D collider)
    {
        if (characterEyes.IsVisible(collider, visionLayers))
        {
            if(collider.gameObject.layer == Reg.playerLayer)
            {
                if (GameManager.Instance.Player.IsHidden)
                {
                    if(Random.Range(1, 100) <= findProbability)
                    {
                        UpdatePosition(collider.transform.position);
                    }
                }
                else
                {
                    UpdatePosition(collider.transform.position);
                }
            }
        }
    }

    private void UpdatePosition(Vector2 position)
    {
        blackboard.LastDetectionPosition = position;
        fullAlertTimer.Start();
    }
}
