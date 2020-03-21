using UnityEngine;

namespace SJ.GameInput
{
    public class CharacterInputAction : InputAction
    {
        [SerializeField]
        private CustomAxisBinding mainKeyBinding, alternativeKeyBinding;

        [SerializeField]
        private LegacyAxisBinding joystickBinding;

        public float AxisValue
        {
            get
            {
                if (mainKeyBinding.WasTriggeredThisFrame)
                    return mainKeyBinding.AxisValue;
                else if (alternativeKeyBinding.WasTriggeredThisFrame)
                    return alternativeKeyBinding.AxisValue;
                else if (joystickBinding != null && joystickBinding.WasTriggeredThisFrame)
                    return joystickBinding.AxisValue;

                return LastTriggered.AxisValue;
            }
        }

        private AxisBinding lastTriggerd;

        private AxisBinding LastTriggered
        {
            get
            {
                if (lastTriggerd == null)
                    lastTriggerd = mainKeyBinding;

                return lastTriggerd;
            }

            set => lastTriggerd = value;
        }

        public KeyCode MainPositive { get => mainKeyBinding.Positive; set => mainKeyBinding.Positive = value; }
        public KeyCode MainNegative { get => mainKeyBinding.Negative; set => mainKeyBinding.Negative = value; }
        public KeyCode AlternativePositive { get => alternativeKeyBinding.Positive; set => alternativeKeyBinding.Positive = value; }
        public KeyCode AlternativeNegative { get => alternativeKeyBinding.Negative; set => alternativeKeyBinding.Negative = value; }

        protected override bool UpdateTriggeredStatus()
        {
            mainKeyBinding.UpdateStatus();
            alternativeKeyBinding.UpdateStatus();

            if(joystickBinding != null)
                joystickBinding.UpdateStatus();

            if(mainKeyBinding.WasTriggeredThisFrame)
            {
                LastTriggered = mainKeyBinding;
                return true;
            }
            else if(alternativeKeyBinding.WasTriggeredThisFrame)
            {
                LastTriggered = alternativeKeyBinding;
                return true;
            }
            else if(joystickBinding != null && joystickBinding.WasTriggeredThisFrame)
            {
                LastTriggered = joystickBinding;
                return true;
            }

            return false;
        }
    }
}