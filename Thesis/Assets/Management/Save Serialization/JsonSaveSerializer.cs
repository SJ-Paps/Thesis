using Newtonsoft.Json;

namespace SJ.Management
{
    public class JsonSaveSerializer : ISaveSerializer
    {
        public struct SaveObject
        {
            public SaveData[] saves;
        }

        public SaveData[] Deserialize(string json)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            SaveObject saveObject = JsonConvert.DeserializeObject<SaveObject>(json, jsonSerializerSettings);

            return saveObject.saves;
        }

        public string Serialize(params SaveData[] saves)
        {
            SaveObject saveObject = default;

            saveObject.saves = saves;

            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            return JsonConvert.SerializeObject(saveObject, jsonSerializerSettings);
        }
    }
}
