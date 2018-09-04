using System;
using UnityEngine;

public class VisionTrigger : MonoBehaviour {

    public event Action<Character> onPlayerDetected;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Character player = collision.GetComponent<Character>();

        if(player != null && player.IsPlayer)
        {
            if(onPlayerDetected != null)
            {
                onPlayerDetected(player);
            }
        }
    }
}
