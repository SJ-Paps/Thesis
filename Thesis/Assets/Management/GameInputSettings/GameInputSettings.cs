using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace SJ.Management
{
    [CreateAssetMenu(menuName = "SJ/Game Input Settings Asset")]
    public class GameInputSettings : ScriptableObject
    {
        [Serializable]
        public struct InputKeyGroup
        {
            [SerializeField]
            public string name;
            [SerializeField]
            public KeyCode main, alternative;
        }

        [SerializeField]
        public bool holdDuckKey, holdWalkKey;

        [SerializeField]
        private InputKeyGroup[] keyGroups;

        public void SetKeysTo(string keyGroupName, KeyCode main, KeyCode alternative)
        {
            for(int i = 0; i < keyGroups.Length; i++)
            {
                if(keyGroups[i].name == keyGroupName)
                {
                    keyGroups[i].main = main;
                    keyGroups[i].alternative = alternative;
                }
            }
        }

        public Dictionary<string, InputKeyGroup> GetGroups()
        {
            return keyGroups.ToDictionary(keyGroup => keyGroup.name, keyGroup => keyGroup);
        }
    }
}