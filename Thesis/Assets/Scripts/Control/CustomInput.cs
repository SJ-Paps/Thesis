using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomInput
{
    public enum Type
    {
        Simple,
        CustomAxis,
        LegacyAxis
    }

    public enum InteractionType
    {
        KeyPressed,
        KeyDown,
        KeyUp
    }

    [SerializeField]
    private Type type;

    [SerializeField]
    private InteractionType interactionType;

    [SerializeField]
    private string name;

    [SerializeField]
    private List<KeyCode> keys;

    [SerializeField]
    private List<KeyCode> positiveKeys, negativeKeys;

    [SerializeField]
    private float gravity, deadZone, sensitivity;

    public string Name => name;
    public float AxisValue { get; private set; }

    public bool IsSimple => type == Type.Simple;
    public bool IsCustomAxis => type == Type.CustomAxis;
    public bool IsLegacyAxis => type == Type.LegacyAxis;

    public CustomInput()
    {
        if (deadZone < 0) deadZone = deadZone * -1;
    }

    public bool WasTriggered()
    {
        if(Input.anyKey)
        {
            switch (type)
            {
                case Type.Simple:
                    return WasSimpleTriggered();
                case Type.LegacyAxis:
                    return WasLegacyAxisTriggered();
                case Type.CustomAxis:
                    return WasCustomAxisTriggered();
            }
        }

        return false;
    }

    private bool WasSimpleTriggered()
    {
        for(int i = 0; i < keys.Count; i++)
        {
            switch(interactionType)
            {
                case InteractionType.KeyDown:
                    if (Input.GetKeyDown(keys[i]))
                        return true;

                    break;

                case InteractionType.KeyPressed:
                    if (Input.GetKey(keys[i]))
                        return true;

                    break;

                case InteractionType.KeyUp:
                    if (Input.GetKeyUp(keys[i]))
                        return true;

                    break;
            }
        }

        return false;
    }

    private bool WasLegacyAxisTriggered()
    {
        AxisValue = Input.GetAxis(name);

        switch(interactionType)
        {
            case InteractionType.KeyDown:
                if (Input.GetButtonDown(name))
                    return true;

                break;

            case InteractionType.KeyPressed:
                if (Input.GetButton(name))
                    return true;

                break;

            case InteractionType.KeyUp:
                if (Input.GetButtonUp(name))
                    return true;

                break;
        }

        return false;
    }

    private bool WasCustomAxisTriggered()
    {
        bool wasTriggered = false;
        bool wasPositiveKey = false;

        for(int i = 0; i < positiveKeys.Count; i++)
        {
            switch(interactionType)
            {
                case InteractionType.KeyDown:
                    if(Input.GetKeyDown(positiveKeys[i]))
                    {
                        wasTriggered = true;
                        wasPositiveKey = true;
                    }
                    break;

                case InteractionType.KeyPressed:
                    if (Input.GetKey(positiveKeys[i]))
                    {
                        wasTriggered = true;
                        wasPositiveKey = true;
                    }
                    break;

                case InteractionType.KeyUp:
                    if (Input.GetKeyUp(positiveKeys[i]))
                    {
                        wasTriggered = true;
                        wasPositiveKey = true;
                    }
                    break;
            }
        }

        for (int i = 0; i < negativeKeys.Count; i++)
        {
            switch (interactionType)
            {
                case InteractionType.KeyDown:
                    if (Input.GetKeyDown(negativeKeys[i]))
                    {
                        wasTriggered = true;
                        wasPositiveKey = false;
                    }
                    break;

                case InteractionType.KeyPressed:
                    if (Input.GetKey(negativeKeys[i]))
                    {
                        wasTriggered = true;
                        wasPositiveKey = false;
                    }
                    break;

                case InteractionType.KeyUp:
                    if (Input.GetKeyUp(negativeKeys[i]))
                    {
                        wasTriggered = true;
                        wasPositiveKey = false;
                    }
                    break;
            }
        }

        if(wasTriggered)
        {
            if(wasPositiveKey)
            {
                switch(interactionType)
                {
                    case InteractionType.KeyUp:
                        AxisValue -= gravity * Time.deltaTime;
                        break;

                    default:
                        AxisValue += sensitivity * Time.deltaTime;
                        break;
                }
            }
            else
            {
                switch (interactionType)
                {
                    case InteractionType.KeyUp:
                        AxisValue += gravity * Time.deltaTime;
                        break;

                    default:
                        AxisValue -= sensitivity * Time.deltaTime;
                        break;
                }
            }
        }
        else
        {
            if (AxisValue < 0)
            {
                AxisValue += gravity * Time.deltaTime;
                if (AxisValue > -deadZone || AxisValue > 0)
                    AxisValue = 0;
            }
            else if(AxisValue > 0)
            {
                AxisValue -= gravity * Time.deltaTime;
                if (AxisValue < deadZone || AxisValue < 0)
                    AxisValue = 0;
            }
        }

        if (AxisValue > 1)
            AxisValue = 1;
        else if (AxisValue < -1)
            AxisValue = -1;

        return wasTriggered;
    }
}
