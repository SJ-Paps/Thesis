using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

public struct SaveData
{
    public string guid;

    public object saveObject;

    public SaveData(string guid, object saveObject)
    {
        this.guid = guid;
        this.saveObject = saveObject;
    }
}

public static class SaveLoadManager
{
    public struct SaveObject
    {
        public SaveData[] saves;
    }

    public static IEnumerator LoadGameCoroutine(string path, Action<SaveData[]> onSuccess, Action onFail, Action onCompletation = null)
    {
        Task<SaveData[]> deserializationTask = DeserializeAsync(path);

        while(deserializationTask.IsCompleted == false)
        {
            yield return null;
        }

        if(deserializationTask.IsFaulted)
        {
            throw deserializationTask.Exception;
            onFail();
        }
        else
        {
            onSuccess(deserializationTask.Result);
        }

        if(onCompletation != null)
        {
            onCompletation();
        }
    }

    private static Task<SaveData[]> DeserializeAsync(string path)
    {
        return Task.Run<SaveData[]>(
            delegate ()
            {
                string json = File.ReadAllText(path);

                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };

                SaveObject saveObject = JsonConvert.DeserializeObject<SaveObject>(json, jsonSerializerSettings);

                return saveObject.saves;
            }
            );
    }

    public static IEnumerator SaveGameCoroutine(string path, ISaveable[] saveables, Action onSuccess, Action onFail, Action onCompletation = null)
    {
        SaveData[] saves = new SaveData[saveables.Length];

        int i = 0;

        foreach(ISaveable saveable in saveables)
        {
            saves[i] = new SaveData(saveable.InstanceGUID, saveable.Save());
            yield return null;
        }

        SaveObject saveObject = default;
        saveObject.saves = saves;

        Task serializationTask = SerializeAsync(path, saveObject);

        while(serializationTask.IsCompleted == false)
        {
            yield return null;
        }

        if(serializationTask.IsFaulted)
        {
            onFail();
        }
        else
        {
            onSuccess();
        }

        if(onCompletation != null)
        {
            onCompletation();
        }
    }

    private static Task SerializeAsync(string path, SaveObject saveObject)
    {
        return Task.Run(
            delegate ()
            {
                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };

                string json = JsonConvert.SerializeObject(saveObject, jsonSerializerSettings);

                File.WriteAllText(path, json);
            }
            );
    }
    
}
