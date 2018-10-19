using UnityEngine;

public class MonobehaviourLogger : MonoBehaviour {

    public void LogEvent(string eventName)
    {
        Logger.AnalyticsCustomEvent(eventName);
    }
}
