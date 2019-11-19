using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Save
{
    public interface ISaveSerializer
    {
        string Serialize(params SaveData[] saves);
        SaveData[] Deserialize(string serializedSaves);
    }
}

