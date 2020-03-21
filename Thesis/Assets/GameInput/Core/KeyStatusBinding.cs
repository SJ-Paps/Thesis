using UnityEngine;

namespace SJ.GameInput
{
    [CreateAssetMenu(menuName = "SJ/Game Input/Input Bindings/Key Status Binding")]
    public class KeyStatusBinding : InputBinding
    {
        [SerializeField]
        private InputInteractionType interactionType;

        [SerializeField]
        private KeyCode key;

        protected override bool UpdateTriggeredStatus()
        {
            if (interactionType == InputInteractionType.KeyDown)
                return Input.GetKeyDown(key);
            else if (interactionType == InputInteractionType.KeyPressed)
                return Input.GetKey(key);
            else
                return Input.GetKeyUp(key);
        }
    }
}