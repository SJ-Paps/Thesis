using UnityEngine;

namespace SJ.GameInput
{
    public abstract class InputBinding : ScriptableObject
    {
        [SerializeField]
        private string bindingName;

        public string BindingName => bindingName;

        public bool WasTriggeredThisFrame { get; private set; }

        public void UpdateStatus()
        {
            WasTriggeredThisFrame = false;

            WasTriggeredThisFrame = UpdateTriggeredStatus();
        }

        protected abstract bool UpdateTriggeredStatus();
    }
}