using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class SaveLoadManager
{
    private static SaveLoadManager instance;

    public static SaveLoadManager GetInstance()
    {
        if(instance == null)
        {
            instance = new SaveLoadManager();
        }

        return instance;
    }

    public void SaveGame()
    {
        ISaveable[] saveables = GameObject.FindObjectsOfType<SJMonoBehaviour>();

        IEnumerable<SaveData> saveData = ForeachSaveDataNotNull(saveables);

        Serialize(saveData);

        CallPostSaveCallbacks(saveables);
    }

    private IEnumerable<SaveData> ForeachSaveDataNotNull(ISaveable[] saveables)
    {
        for(int i = 0; i < saveables.Length; i++)
        {
            ISaveable saveable = saveables[i];

            SaveData data = saveable.Save();

            if(data != null)
            {
                yield return data;
            }
        }
    }

    private void CallPostSaveCallbacks(ISaveable[] saveables)
    {
        for(int i = 0; i < saveables.Length; i++)
        {
            saveables[i].PostSaveCallback();
        }
    }

    public void LoadGame()
    {
        SJMonoBehaviour[] saveables = GameObject.FindObjectsOfType<SJMonoBehaviour>();

        SaveData[] saves = Deserialize(Path.Combine(Application.persistentDataPath, "save.sj"));

        for (int i = 0; i < saveables.Length; i++)
        {
            SJMonoBehaviour saveable = saveables[i];

            for(int j = 0; j < saves.Length; j++)
            {
                SaveData data = saves[j];
                
                if(data.ClassName == saveable.ClassName)
                {
                    saveable.Load(data);
                    break;
                }
            }
        }
    }

    private void Serialize(IEnumerable<SaveData> saves)
    {
        string json = JsonConvert.SerializeObject(saves);

        File.WriteAllText(Path.Combine(Application.persistentDataPath, "save.sj"), json);
    }

    private SaveData[] Deserialize(string path)
    {
        string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, "save.sj"));

        return JsonConvert.DeserializeObject<SaveData[]>(json);
    }
    
}
