using UnityEngine;

namespace SJ.GameInput
{
    [CreateAssetMenu(menuName = "SJ/Input Actions/Toggle Hold Key Action")]
    public class ToggleHoldKeyAction : InputAction, ISerializationCallbackReceiver
    {
        [SerializeField]
        private bool hold;

        [SerializeField]
        private KeyCode[] defaultKeys;

        public bool Hold { get; set; }
        private KeyCode[] Keys { get; set; }

        private bool wasHolding;

        public void OnAfterDeserialize()
        {
            SetKeys(defaultKeys);
            Hold = hold;
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
            if(Hold)
            {
                bool holding = AnyKeyHeld();

                if (wasHolding && !holding)
                {
                    wasHolding = false;
                    return true;
                }
                else if(!wasHolding && holding)
                {
                    wasHolding = true;
                    return true;
                }
            }
            else
            {
                return AnyKeyDown();
            }

            return false;
        }

        private bool AnyKeyHeld()
        {
            for (int i = 0; i < Keys.Length; i++)
                if (Input.GetKey(Keys[i]))
                    return true;

            return false;
        }

        private bool AnyKeyDown()
        {
            for (int i = 0; i < Keys.Length; i++)
                if (Input.GetKeyDown(Keys[i]))
                    return true;

            return false;
        }
    }
}