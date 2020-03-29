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

            public InputKeyGroup(string name, KeyCode main, KeyCode alternative)
            {
                this.name = name;
                this.main = main;
                this.alternative = alternative;
            }
        }

        [SerializeField]
        public bool holdDuckKey, holdWalkKey;

        [SerializeField]
        private List<InputKeyGroup> keyGroups;

#if UNITY_EDITOR
        private void Awake()
        {
            if (keyGroups == null)
                keyGroups = new List<InputKeyGroup>();
        }

        public void AddGroup(string name, KeyCode main, KeyCode alternative)
        {
            keyGroups.Add(new InputKeyGroup(name, main, alternative));
        }
#endif

        public void SetKeysTo(string keyGroupName, KeyCode main, KeyCode alternative)
        {
            for(int i = 0; i < keyGroups.Count; i++)
            {
                if(keyGroups[i].name == keyGroupName)
                {
                    keyGroups[i] = new InputKeyGroup(keyGroupName, main, alternative);
                }
            }
        }

        public Dictionary<string, InputKeyGroup> GetGroups()
        {
            return keyGroups.ToDictionary(keyGroup => keyGroup.name, keyGroup => keyGroup);
        }
    }
}