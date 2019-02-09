using SAM.Timers;
using System;
using UnityEngine;

[Serializable]
public class XenophobicAlertfulState : XenophobicIAState
{
    [SerializeField]
    private Vector2 eyeSize = new Vector2(12, 5);

    private Eyes characterEyes;

    private SyncTimer fullAlertTimer;
    private SyncTimer playerDetectedTimer;

    [SerializeField]
    private float playerDetectedTime = 6f, fullAlertTime = 10f, hiddenDetectionDistance = 1.5f;

    private Action<Collider2D> onSomethingDetectedDelegate;

    private int targetLayers;

    [SerializeField]
    private float baseFindProbability = 60, maxFindProbability = 100;

    private float findProbability;

    public XenophobicAlertfulState(XenophobicIAController.State state, string debugName) : base(state, debugName)
    {
        characterEyes = controller.SlaveEyes;

        fullAlertTimer = new SyncTimer();
        fullAlertTimer.Interval = fullAlertTime;
        //fullAlertTimer.onTick += CalmDown;

        playerDetectedTimer = new SyncTimer();
        playerDetectedTimer.Interval = playerDetectedTime;
        //playerDetectedTimer.onTick += LosePlayerData;

        //onSomethingDetectedDelegate += AnalyzeDetection;

        findProbability = baseFindProbability;

        targetLayers = (1 << Reg.playerLayer);
    }

    /*protected override void OnEnter()
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
            if (characterEyes.IsVisible(collider, Reg.walkableLayerMask, targetLayers))
            {
                if (blackboard.PlayerData == null && GameManager.Instance.GetPlayer().IsHidden)
                {
                    if(characterEyes.IsNear(collider, Reg.walkableLayerMask, targetLayers, hiddenDetectionDistance))
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
    }*/
}
