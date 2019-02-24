using System;

public struct RangeInt
{
    private bool throwExceptionOnOverflow;

    private int min;
    private int max;
    private int _value;

    public int Value
    {
        get
        {
            return _value;
        }

        set
        {
            if(_value < min)
            {
                if(throwExceptionOnOverflow)
                {
                    throw new OverflowException("value is less than minimum permitted");
                }
                else
                {
                    _value = min;
                }
            }
            else if(_value > max)
            {
                if(throwExceptionOnOverflow)
                {
                    throw new OverflowException("value is greater than maximum permitted");
                }
                else
                {
                    _value = max;
                }
            }

        }
    }

    public RangeInt(int min, int max, int value, bool throwExceptionOnOverflow)
    {
        this.min = min;
        this.max = max;
        _value = 0;
        this.throwExceptionOnOverflow = throwExceptionOnOverflow;

        Value = value;
    }

    public static implicit operator int(RangeInt range)
    {
        return range.Value;
    }
}

public struct RangeFloat
{
    private bool throwExceptionOnOverflow;

    private float min;
    private float max;
    private float _value;

    public float Value
    {
        get
        {
            return _value;
        }

        set
        {
            if (_value < min)
            {
                if (throwExceptionOnOverflow)
                {
                    throw new OverflowException("value is less than minimum permitted");
                }
                else
                {
                    _value = min;
                }
            }
            else if (_value > max)
            {
                if (throwExceptionOnOverflow)
                {
                    throw new OverflowException("value is greater than maximum permitted");
                }
                else
                {
                    _value = max;
                }
            }

        }
    }

    public RangeFloat(float min, float max, float value, bool throwExceptionOnOverflow)
    {
        this.min = min;
        this.max = max;
        _value = 0;
        this.throwExceptionOnOverflow = throwExceptionOnOverflow;

        Value = value;
    }

    public static implicit operator float(RangeFloat range)
    {
        return range.Value;
    }
}
