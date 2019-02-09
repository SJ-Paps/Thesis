using SAM.Timers;
using System;
using UnityEngine;

public class XenophobicAwareState : XenophobicIAState
{
    private Eyes characterEyes;

    private SyncTimer awareTimer;
    
    private Vector2 eyesSize = new Vector2(9, 5);
    
    private float fullAlertDetectionDistance = 6f, hiddenDetectionDistance = 1f, awareTime = 4f;

    private Action<Collider2D> onSomethingDetectedStayDelegate;
    
    private int findProbability = 15;
    
    private int targetLayers;

    public XenophobicAwareState(XenophobicIAController.State state, string debugName) : base(state, debugName)
    {
        characterEyes = controller.SlaveEyes;

        awareTimer = new SyncTimer();
        awareTimer.Interval = awareTime;
        /*awareTimer.onTick += CalmDown;

        onSomethingDetectedStayDelegate += AnalyzeDetection;*/

        targetLayers = (1 << Reg.playerLayer);
    }

    /*protected override void OnEnter()
    {
        if (characterEyes != null)
        {
            awareTimer.Start();

            characterEyes.Trigger2D.ChangeSize(eyesSize);

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

    

    private void UpdatePosition(Vector2 position)
    {
        blackboard.LastDetectionPosition = position;
        awareTimer.Start();
    }

    private void AnalyzeDetection(Collider2D collider)
    {
        if (collider.gameObject.layer == Reg.playerLayer)
        {
            if(characterEyes.IsVisible(collider, Reg.walkableLayerMask, targetLayers))
            {
                if (GameManager.Instance.GetPlayer().IsHidden)
                {
                    if(Random.Range(1, 100) <= findProbability)
                    {
                        UpdatePosition(collider.transform.position);

                        if(characterEyes.IsNear(collider, Reg.walkableLayerMask, targetLayers, hiddenDetectionDistance))
                        {
                            SetFullAlert(collider.transform.position);
                        }
                    }
                }
                else
                {
                    UpdatePosition(collider.transform.position);

                    if (characterEyes.IsNear(collider, Reg.walkableLayerMask, targetLayers, fullAlertDetectionDistance))
                    {
                        SetFullAlert(collider.transform.position);
                    }
                }
            }
        }
    }

    private void SetFullAlert(Vector2 lastSeekedPosition)
    {
        blackboard.LastDetectionPosition = lastSeekedPosition;
        stateMachine.Trigger(XenophobicIAController.Trigger.SetFullAlert);
    }

    private void CalmDown(SyncTimer timer)
    {
        CalmDown();
    }

    private void CalmDown()
    {
        stateMachine.Trigger(XenophobicIAController.Trigger.CalmDown);
    }*/
}
