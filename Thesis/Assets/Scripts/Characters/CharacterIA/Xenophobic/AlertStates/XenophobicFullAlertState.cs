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
    private float fullAlertTime = 20f;

    private SyncTimer playerDetectedTimer;
    private float playerDetectedTime = 6f;

    private float hiddenDetectionDistance = 1.5f;

    private Character playerData;

    private Action<Collider2D> onSomethingDetectedDelegate;

    private int visionLayers = (1 << Reg.floorLayer) | (1 << Reg.playerLayer) | (1 << Reg.objectLayer);

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
            if (characterEyes.IsVisible(collider, visionLayers))
            {
                if (playerData == null && GameManager.Instance.Player.IsHidden)
                {
                    if(characterEyes.IsNear(collider, visionLayers, hiddenDetectionDistance))
                    {
                        if (Random.Range(1, 100) <= findProbability)
                        {
                            SetPlayerAsFound(GameManager.Instance.Player);
                        }
                    }
                }
                else
                {
                    SetPlayerAsFound(GameManager.Instance.Player);
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
        playerData = player;
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
