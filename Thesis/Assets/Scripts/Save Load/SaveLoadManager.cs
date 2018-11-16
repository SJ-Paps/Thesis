using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System;

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

    private const char objectSeparator = '!';

    public void SaveGame()
    {
        ISaveable[] saveables = GameObject.FindObjectsOfType<SJMonoBehaviour>().OfType<ISaveable>().ToArray();
        
        SaveData[] saveData = new SaveData[saveables.Length];

        for(int i = 0; i < saveData.Length; i++)
        {
            saveData[i] = saveables[i].Save();
        }

        FileStream stream = File.OpenWrite(Path.Combine(Application.persistentDataPath, "save.sj"));

        Serialize(saveData, stream);

        stream.Close();
    }

    public void LoadGame()
    {
        ISaveable[] saveables = GameObject.FindObjectsOfType<SJMonoBehaviour>().OfType<ISaveable>().ToArray();

        saveables[0].Load(Deserialize(Path.Combine(Application.persistentDataPath, "save.sj"))[0]);
    }

    private void Serialize(SaveData[] saves, Stream target)
    {
        using (StreamWriter writer = new StreamWriter(target))
        {
            for (int i = 0; i < saves.Length; i++)
            {
                SaveData current = saves[i];

                foreach (KeyValuePair<string, string> value in current)
                {
                    writer.Write(value.Key + ':' + value.Value);
                    writer.WriteLine();
                }

                if(i != saves.Length - 1)
                {
                    writer.Write(objectSeparator);
                    writer.WriteLine();
                }
            }
        }
    }

    private SaveData[] Deserialize(string path)
    {
        List<SaveData> saves = new List<SaveData>();

        string[] lines = File.ReadAllLines(path);

        SaveData data = new SaveData();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            Debug.Log("LINE: " + line);
            Debug.Log("LENGTH: " + line.Length);

            if (line != objectSeparator.ToString())
            {
                int separatorIndex = line.IndexOf(':');

                Debug.Log(separatorIndex);

                string name = line.Substring(0, separatorIndex);
                Debug.Log(name);
                string value = line.Substring(separatorIndex + 1, line.Length - 2);
                Debug.Log(value);

                data.AddValue(name, value);
            }
            else
            {
                saves.Add(data);
                data = new SaveData();
            }
        }

        saves.Add(data);

        return saves.ToArray();
    }
    
}
