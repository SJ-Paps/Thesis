using SAM.FSM;
using SAM.Timers;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class XenophobicFullAlertState : XenophobicIAState
{
    private Vector2 eyeSize = new Vector2(12, 5);

    private Eyes characterEyes;

    private SyncTimer fullAlertTimer;
    private float fullAlertTime = 10f;

    private SyncTimer playerDetectedTimer;
    private float playerDetectedTime = 6f;

    private float hiddenDetectionDistance = 1.5f;

    private Action<Collider2D> onSomethingDetectedDelegate;

    private int visionLayers = (1 << Reg.floorLayer) | (1 << Reg.objectLayer);
    private int targetLayers = (1 << Reg.playerLayer);

    private const float baseFindProbability = 60;
    private const float maxFindProbability = 100;
    private float findProbability = baseFindProbability;

    public XenophobicFullAlertState(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state, controller, blackboard)
    {
        characterEyes = controller.SlaveEyes;

        fullAlertTimer = new SyncTimer();
        fullAlertTimer.Interval = fullAlertTime;
        fullAlertTimer.onTick += CalmDown;

        playerDetectedTimer = new SyncTimer();
        playerDetectedTimer.Interval = playerDetectedTime;
        playerDetectedTimer.onTick += LosePlayerData;

        onSomethingDetectedDelegate += AnalyzeDetection;
    }

    protected override void OnEnter()
    {
        if (characterEyes != null)
        {
            characterEyes.Trigger2D.ChangeSize(eyeSize);

            characterEyes.Trigger2D.onStay += onSomethingDetectedDelegate;

            fullAlertTimer.Start();
        }
    }

    protected override void OnUpdate()
    {
        fullAlertTimer.Update(Time.deltaTime);
        playerDetectedTimer.Update(Time.deltaTime);
    }

    protected override void OnExit()
    {
        if (characterEyes != null)
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
        if (collider.gameObject.layer == Reg.playerLayer)
        {
            if (characterEyes.IsVisible(collider, visionLayers, targetLayers))
            {
                if (blackboard.PlayerData == null && GameManager.Instance.GetPlayer().IsHidden)
                {
                    if(characterEyes.IsNear(collider, visionLayers, targetLayers, hiddenDetectionDistance))
                    {
                        if (Random.Range(1, 100) <= findProbability)
                        {
                            SetPlayerAsFound(GameManager.Instance.GetPlayer());
                        }
                    }
                }
                else
                {
                    SetPlayerAsFound(GameManager.Instance.GetPlayer());
                }
            }
        }
    }

    private void UpdatePosition(Vector2 position)
    {
        blackboard.LastDetectionPosition = position;
        fullAlertTimer.Start();
    }

    private void LosePlayerData(SyncTimer timer)
    {
        UpdatePlayerData(null);

        findProbability = baseFindProbability;

        EditorDebug.Log("XENOPHOBIC " + controller.Slave.name + " HAS LOST THE PLAYER");
    }

    private void UpdatePlayerData(Character player)
    {
        blackboard.PlayerData = player;
    }

    private void SetPlayerAsFound(Character player)
    {
        player.NotifyDetection();
        UpdatePosition(player.transform.position);
        UpdatePlayerData(player);

        findProbability = maxFindProbability;
        playerDetectedTimer.Start();
    }
}
