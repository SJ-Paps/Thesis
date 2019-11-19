using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Save
{
    public struct SaveData
    {
        public string identifier;

        public object saveObject;

        public SaveData(string identifier, object saveObject)
        {
            this.identifier = identifier;
            this.saveObject = saveObject;
        }
    }
}
