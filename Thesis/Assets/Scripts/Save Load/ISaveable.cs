using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    SaveData Save();

    void PostSaveCallback();

    void Load(SaveData data);

    void PostLoadCallback();

}