using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public class SaveData
{
    [JsonProperty]
    private Dictionary<string, string> values;

    [JsonProperty]
    private Guid guid;

    [JsonProperty]
    private string prefabName;
    
    public int ValueCount
    {
        get
        {
            return values.Count;
        }
    }

    public Guid GUID
    {
        get
        {
            return guid;
        }
    }

    public string PrefabName
    {
        get
        {

            return prefabName;
        }
    }

    public SaveData(Guid guid, string prefabName)
    {
        this.guid = guid;
        this.prefabName = prefabName;

        values = new Dictionary<string, string>();
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
