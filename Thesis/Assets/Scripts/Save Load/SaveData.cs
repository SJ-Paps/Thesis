using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public class SaveData
{
    [JsonProperty]
    private string className;

    [JsonProperty]
    private Dictionary<string, string> values;
    
    public int ValueCount
    {
        get
        {
            return values.Count;
        }
    }

    public SaveData(string className)
    {
        this.className = className;

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
}
