using Newtonsoft.Json;
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

    public static Task SerializeAsync(string path, SaveData[] saves)
    {
        SaveObject saveObject = default;

        saveObject.saves = saves;

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
