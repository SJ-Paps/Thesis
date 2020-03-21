using UnityEngine;

namespace SJ.GameInput
{
    public abstract class InputAction : ScriptableObject
    {
        [SerializeField]
        private string actionName;

        public string ActionName => actionName;

        public bool WasTriggeredThisFrame { get; protected set; }

        public void UpdateStatus()
        {
            WasTriggeredThisFrame = false;

            WasTriggeredThisFrame = UpdateTriggeredStatus();
        }

        protected abstract bool UpdateTriggeredStatus();
    }
}