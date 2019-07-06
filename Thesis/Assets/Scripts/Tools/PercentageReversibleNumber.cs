using System.Collections.Generic;
using System;

public class PercentageReversibleNumber
{
    public float BaseValue { get; private set; }
    public float CurrentValue { get; private set; }

    private List<float> constraints;

    public PercentageReversibleNumber()
    {
        constraints = new List<float>();

        AddDefaultConstraint();
    }

    public PercentageReversibleNumber(float baseValue) : this()
    {
        BaseValue = baseValue;

        Recalculate();
    }

    public void SetBaseValue(float value)
    {
        BaseValue = value;

        Recalculate();
    }

    public int AddPercentageConstraint(float percentage)
    {
        constraints.Add(percentage);

        Recalculate();

        return constraints.Count - 1;
    }

    public void RemovePercentageConstraint(int id)
    {
        if(id < 1 || id > constraints.Count - 1)
        {
            return;
        }

        constraints.RemoveAt(id);

        Recalculate();
    }

    public void RemoveAll()
    {
        constraints.Clear();

        AddDefaultConstraint();

        Recalculate();
    }

    private void Recalculate()
    {
        CurrentValue = BaseValue;

        for(int i = 0; i < constraints.Count; i++)
        {
            CurrentValue = (constraints[i] * CurrentValue) / 100;
        }
    }

    private void AddDefaultConstraint()
    {
        constraints.Add(100);
    }

    public static implicit operator float(PercentageReversibleNumber number)
    {
        return number.CurrentValue;
    }
}
