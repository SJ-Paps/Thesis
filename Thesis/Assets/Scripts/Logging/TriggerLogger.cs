using UnityEngine;
using UnityEngine.Events;

public class TriggerLogger : SJBoxCollider2D
{
    [SerializeField]
    private UnityEvent onTriggerEnter;

    protected override void SJStart()
    {
        base.SJStart();

        onEnteredTrigger += OnEnter;
    }

    private void OnEnter(Collider2D collision)
    {
        onTriggerEnter.Invoke();
    }

    public void LogAnalyticsCustomEvent(string eventName)
    {
        Logger.AnalyticsCustomEvent(eventName);
    }
}
