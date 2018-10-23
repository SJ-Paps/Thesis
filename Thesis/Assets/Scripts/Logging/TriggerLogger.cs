using UnityEngine;
using UnityEngine.Events;

public class TriggerLogger : Trigger2D
{
    [SerializeField]
    private UnityEvent onTriggerEnter;

    void Awake()
    {
        InnerCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        onEntered += OnEnter;
    }

    private void OnEnter(Collider2D collision)
    {
        onTriggerEnter.Invoke();
    }

    public override void ChangeSize(Vector2 size)
    {
        ((BoxCollider2D)InnerCollider).size = size;
    }

    public void LogAnalyticsCustomEvent(string eventName)
    {
        Logger.AnalyticsCustomEvent(eventName);
    }
}
