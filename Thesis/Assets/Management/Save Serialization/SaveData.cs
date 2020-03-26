using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SJ.Management
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
