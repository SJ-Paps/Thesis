using System;
using UnityEngine;

public class TriggerStay : MonoBehaviour {

    public event Action<Collider2D> onSomethingDetected;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (onSomethingDetected != null)
        {
            onSomethingDetected(collision);
        }
    }
}
