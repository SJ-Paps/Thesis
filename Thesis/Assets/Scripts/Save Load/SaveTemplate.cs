using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTemplate
{
    public string instanceGUID;
    public string prefabName;

    public object save;

    public SaveTemplate(SJMonoBehaviourSaveable obj, object save)
    {
        instanceGUID = obj.InstanceGUID;
        prefabName = obj.PrefabName;

        this.save = save;
    }

    public SaveTemplate()
    {

    }
}
