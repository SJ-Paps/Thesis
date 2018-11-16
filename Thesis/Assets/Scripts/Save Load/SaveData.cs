using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveData : IEnumerable<KeyValuePair<string, string>>
{
    private Dictionary<string, string> values;

    public int ValueCount
    {
        get
        {
            return values.Count;
        }
    }

    public SaveData()
    {
        values = new Dictionary<string, string>();
    }

    public void AddValue(string name, object value)
    {
        values.Add(name, value.ToString());
    }

    public void RemoveValue(string name)
    {
        values.Remove(name);
    }

    public string Get(string name)
    {
        return values[name];
    }

    public T GetAs<T>(string name)
    {
        return (T)Convert.ChangeType(values[name], typeof(T), System.Globalization.CultureInfo.InvariantCulture);
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
