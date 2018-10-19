using UnityEngine;
using UnityEngine.Events;

public class TriggerLogger : MonobehaviourLogger
{
    [SerializeField]
    private UnityEvent onTriggerEnter;

    void OnTriggerEnter2D(Collider2D collision)
    {
        EditorDebug.Log("ENTRE");
        onTriggerEnter.Invoke();
    }
}
