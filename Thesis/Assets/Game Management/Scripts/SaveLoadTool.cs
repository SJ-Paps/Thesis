﻿using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;


public struct SaveData
{
    public string identifier;

    public object saveObject;

    public SaveData(string identifier, object saveObject)
    {
        this.identifier = identifier;
        this.saveObject = saveObject;
    }
}

public static class SaveLoadTool
{
    public struct SaveObject
    {
        public SaveData[] saves;
    }

    public static Task<SaveData[]> DeserializeAsync(string path)
    {
        return Task.Run<SaveData[]>(
            delegate ()
            {
                return Deserialize(path);
            }
            );
    }

    public static SaveData[] Deserialize(string path)
    {
        string json = File.ReadAllText(path);

        var jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        SaveObject saveObject = JsonConvert.DeserializeObject<SaveObject>(json, jsonSerializerSettings);

        return saveObject.saves;
    }

    public static Task SerializeAsync(string path, params SaveData[] saves)
    {
        return Task.Run(
            delegate ()
            {
                Serialize(path, saves);
            }
            );
    }

    public static void Serialize(string path, params SaveData[] saves)
    {
        SaveObject saveObject = default;

        saveObject.saves = saves;

        var jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto
        };

        string json = JsonConvert.SerializeObject(saveObject, jsonSerializerSettings);

        File.WriteAllText(path, json);
    }

}