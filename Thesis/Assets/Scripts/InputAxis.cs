using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputAxis
{

    [SerializeField]
    private KeyValuePair<KeyCode, KeyCode> positiveButtons;
    [SerializeField]
    private KeyValuePair<KeyCode, KeyCode> negativeButtons;

    [SerializeField]
    private string name;

    public string Name
    {
        get
        {
            return name;
        }
    }

    public float gravity = 0;
    public float dead = 0.1f;
    public float sensitivity = 1;

    private float axis;

    public bool snap = true;
    public bool invert = false;

    public InputAxis(string _name)
    {
        name = _name;
    }

    public InputAxis(string _name, KeyCode positive, KeyCode negative, KeyCode altPositive, KeyCode altNegative) : this(_name)
    {
        positiveButtons = new KeyValuePair<KeyCode, KeyCode>(positive, altPositive);
        negativeButtons = new KeyValuePair<KeyCode, KeyCode>(negative, altNegative);
    }

    public InputAxis(string _name, KeyCode positive, KeyCode negative) : this(_name, positive, negative, KeyCode.None, KeyCode.None)
    {

    }



    private void SetKeyPair(KeyValuePair<KeyCode, KeyCode> pair, KeyCode main, KeyCode alternative)
    {
        pair = new KeyValuePair<KeyCode, KeyCode>(main, alternative);
    }

    public void SetMainPositive(KeyCode key)
    {
        SetKeyPair(positiveButtons, key, positiveButtons.Value);
    }

    public void SetAlternativePositive(KeyCode key)
    {
        SetKeyPair(positiveButtons, positiveButtons.Key, key);
    }

    public void SetMainNegative(KeyCode key)
    {
        SetKeyPair(negativeButtons, key, negativeButtons.Value);
    }

    public void SetAlternativeNegative(KeyCode key)
    {
        SetKeyPair(negativeButtons, negativeButtons.Key, key);
    }

    public void SetBothPositive(KeyCode main, KeyCode alternative)
    {
        SetKeyPair(positiveButtons, main, alternative);
    }

    public void SetBothNegative(KeyCode main, KeyCode alternative)
    {
        SetKeyPair(negativeButtons, main, alternative);
    }
    
}
