using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public class SaveData
{
    [JsonProperty]
    public string ClassName { get; private set; }

    [JsonProperty]
    public bool shouldBeInstanciatedOnLoad;

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
        ClassName = className;

        values = new Dictionary<string, string>();
    }

    public SaveData(string className, bool shouldBeInstanciatedOnLoad) : this(className)
    {
        this.shouldBeInstanciatedOnLoad = shouldBeInstanciatedOnLoad;
    }

    [JsonConstructor]
    private SaveData() { }

    public void AddValue(string name, object value)
    {
        values.Add(name, string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", value));
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
