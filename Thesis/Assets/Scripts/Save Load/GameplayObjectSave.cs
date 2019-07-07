using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayObjectSave
{
    public string instanceGUID;
    public string prefabName;

    public object save;

    public GameplayObjectSave(SJMonoBehaviourSaveable obj, object save)
    {
        instanceGUID = obj.InstanceGUID;
        prefabName = obj.PrefabName;

        this.save = save;
    }

    public GameplayObjectSave()
    {

    }
}
