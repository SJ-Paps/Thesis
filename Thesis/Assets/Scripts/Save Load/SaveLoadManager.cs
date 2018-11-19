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

    private List<ISaveable> saveables;

    private static string saveFilePath = Path.Combine(Application.persistentDataPath, "save.sj");

    private SaveLoadManager()
    {
        saveables = new List<ISaveable>();
    }

    public void Subscribe(ISaveable saveable)
    {
        if(saveables.Contains(saveable) == false)
        {
            saveables.Add(saveable);
        }
    }

    public void Desubscribe(ISaveable saveable)
    {
        saveables.Remove(saveable);
    }

    public void SaveGame()
    {
        IEnumerable<SaveData> saveData = ForeachSaveDataNotNull(saveables);

        Serialize(saveData);

        CallPostSaveCallbacks(saveables);

        EditorDebug.Log("GUARDADO!");
    }

    private IEnumerable<SaveData> ForeachSaveDataNotNull(List<ISaveable> saveables)
    {
        for(int i = 0; i < saveables.Count; i++)
        {
            ISaveable saveable = saveables[i];

            SaveData data = saveable.Save();

            if(data != null)
            {
                yield return data;
            }
        }
    }

    private void CallPostSaveCallbacks(List<ISaveable> saveables)
    {
        for(int i = 0; i < saveables.Count; i++)
        {
            saveables[i].PostSaveCallback();
        }
    }

    public SaveData[] LoadSaves()
    {
        if(SaveFileExists())
        {
            return Deserialize(saveFilePath);
        }

        return null;
    }

    public bool SaveFileExists()
    {
        return File.Exists(saveFilePath);
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
