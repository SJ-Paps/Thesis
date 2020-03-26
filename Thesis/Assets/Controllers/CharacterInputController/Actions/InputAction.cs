using UnityEngine;

namespace SJ.GameInput
{
    public abstract class InputAction : ScriptableObject
    {
        [SerializeField]
        private string actionName;

        public string ActionName => actionName;

        public abstract bool WasTriggeredThisFrame();
    }
}