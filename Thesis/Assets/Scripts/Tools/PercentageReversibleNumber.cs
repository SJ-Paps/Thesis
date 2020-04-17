using System.Collections.Generic;
using System;

public class PercentageReversibleNumber
{
    public float BaseValue { get; private set; }
    public float CurrentValue { get; private set; }

    private Dictionary<int, float> constraints = new Dictionary<int, float>();

    public PercentageReversibleNumber(float baseValue)
    {
        BaseValue = baseValue;

        Recalculate();
    }

    public void SetBaseValue(float value)
    {
        BaseValue = value;

        Recalculate();
    }

    public int AddConstraint(float percentage)
    {
        var id = Guid.NewGuid().GetHashCode();

        constraints.Add(id, percentage);

        Recalculate();

        return id;
    }

    public void RemoveConstraint(int id)
    {
        if(constraints.Remove(id))
            Recalculate();
    }

    public void RemoveAll()
    {
        constraints.Clear();

        Recalculate();
    }

    private void Recalculate()
    {
        CurrentValue = BaseValue;

        foreach (var constraint in constraints)
            CurrentValue = (constraint.Value * CurrentValue) / 100;
    }

    public static implicit operator float(PercentageReversibleNumber number)
    {
        return number.CurrentValue;
    }
}
