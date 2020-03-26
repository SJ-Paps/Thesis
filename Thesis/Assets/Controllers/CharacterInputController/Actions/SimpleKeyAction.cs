using UnityEngine;

namespace SJ.GameInput
{
    [CreateAssetMenu(menuName = "SJ/Input Actions/Simple Key Action")]
    public class SimpleKeyAction : InputAction, ISerializationCallbackReceiver
    {
        [SerializeField]
        private InputInteractionType interactionType;

        [SerializeField]
        private KeyCode[] defaultKeys;

        private KeyCode[] Keys { get; set; }

        public InputInteractionType InteractionType { get; set; }

        public void OnAfterDeserialize()
        {
            SetKeys(defaultKeys);
            InteractionType = interactionType;
        }

        public void OnBeforeSerialize()
        {

        }

        public void SetKeys(params KeyCode[] keys)
        {
            Keys = new KeyCode[keys.Length];
            keys.CopyTo(this.Keys, 0);
        }

        public override bool WasTriggeredThisFrame()
        {
            for (int i = 0; i < Keys.Length; i++)
            {
                var current = Keys[i];

                if (InteractionType == InputInteractionType.KeyDown && Input.GetKeyDown(current))
                    return true;
                else if (InteractionType == InputInteractionType.KeyPressed && Input.GetKey(current))
                    return true;
                else if (Input.GetKeyUp(current))
                    return true;
            }

            return false;
        }
    }
}