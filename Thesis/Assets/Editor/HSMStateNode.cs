using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSMStateNode
{
    public Vector2 position = new Vector2();
    public bool available = true;

    public void resetValues()
    {
        available = true;
    }
}
