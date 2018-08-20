using UnityEngine;
using System.Collections.Generic;

public class AxisButtonPair
{
    public KeyCode Positive { get; private set; }
    public KeyCode Negative { get; private set; }

    public string Name { get; private set; }

    public AxisButtonPair(string name, KeyCode positive, KeyCode negative)
    {
        Name = name;
        Positive = positive;
        Negative = negative;
    }
}

public class InputAxis
{
    private List<AxisButtonPair> buttonPairs;

    public string Name { get; private set; }

    public float gravity;
    public float dead;
    public float sensitivity;

    private float axis;

    public bool snap;
    public bool invert;

    public InputAxis(string name, float gravity, float dead, float sensitivity, bool snap, bool invert, AxisButtonPair defaultButtonPair)
    {
        buttonPairs = new List<AxisButtonPair>();

        Name = name;
        this.gravity = gravity;
        this.dead = dead;
        this.sensitivity = sensitivity;
        this.snap = snap;
        this.invert = invert;
        axis = 0;
    }

    public void AddButtonPair(AxisButtonPair buttonPair)
    {
        buttonPairs.Add(buttonPair);
    }

    public void RemoveButtonPair(AxisButtonPair buttonPair)
    {
        buttonPairs.Remove(buttonPair);
    }

    public void UpdateAxis()
    {

    }

    public AxisButtonPair[] GetAllButtonPairs()
    {
        return buttonPairs.ToArray();
    }

}
