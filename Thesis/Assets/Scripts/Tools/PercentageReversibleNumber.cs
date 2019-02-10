using System.Collections.Generic;

public class PercentageReversibleNumber
{
    public double BaseValue { get; private set; }
    public double CurrentValue { get; private set; }

    private List<double> constraints;

    public PercentageReversibleNumber()
    {
        constraints = new List<double>();

        AddDefaultConstraint();
    }

    public PercentageReversibleNumber(double baseValue) : this()
    {
        BaseValue = baseValue;

        Recalculate();
    }

    public PercentageReversibleNumber(float baseValue) : this((double)baseValue)
    {

    }

    public void SetBaseValue(double value)
    {
        BaseValue = value;

        Recalculate();
    }

    public void SetBaseValue(float value)
    {
        SetBaseValue((double)value);
    }

    public int AddPercentageConstraint(double percentage)
    {
        constraints.Add(percentage);

        Recalculate();

        return constraints.Count - 1;
    }

    public int AddPercentageConstraint(float percentage)
    {
        return AddPercentageConstraint((double)percentage);
    }

    public void ReversePercentageConstraint(int id)
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
}
