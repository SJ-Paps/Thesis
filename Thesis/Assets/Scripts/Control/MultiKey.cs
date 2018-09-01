using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public struct MultiKey
{
    [SerializeField]
    private bool isAxis;

    [SerializeField]
    private bool lessThanZero;

    [SerializeField]
    private string name;

    [SerializeField]
    private List<KeyCode> keys;

    public string Name
    {
        get
        {
            return name;
        }

        private set
        {
            name = value;
        }

    }

    public int KeyCount
    {
        get
        {
            return keys.Count;
        }
    }

    public MultiKey(string name, bool isAxis, bool lessThanZero)
    {
        this.name = name;
        keys = new List<KeyCode>();
        this.isAxis = isAxis;
        this.lessThanZero = lessThanZero;
    }

    public void AddKey(KeyCode key)
    {
        if (keys.Contains(key) == false)
        {
            keys.Add(key);
        }
    }

    public void RemoveKey(KeyCode key)
    {
        keys.Remove(key);
    }

    public KeyCode[] GetKeys()
    {
        return keys.ToArray();
    }

    public bool WasTriggered()
    {
        foreach (KeyCode k in keys)
        {
            if (Input.GetKey(k))
            {
                return true;
            }
        }

        if (isAxis)
        {
            float axisValue = Input.GetAxis(name);

            if ((lessThanZero == true && axisValue < 0) || (lessThanZero == false && axisValue > 0))
            {
                return true;
            }
        }

        return false;

    }
}
