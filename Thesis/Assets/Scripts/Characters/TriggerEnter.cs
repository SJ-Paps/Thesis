using System;
using UnityEngine;

public class TriggerEnter : MonoBehaviour {

    public event Action<Collider2D> onSomethingDetected;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(onSomethingDetected != null)
        {
            onSomethingDetected(collision);
        }
    }
}
