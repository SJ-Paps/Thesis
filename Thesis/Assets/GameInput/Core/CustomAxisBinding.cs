using UnityEngine;

namespace SJ.GameInput
{
    [CreateAssetMenu(menuName = "SJ/Game Input/Input Bindings/Custom Axis Binding")]
    public class CustomAxisBinding : AxisBinding
    {
        [SerializeField]
        private KeyCode positive, negative;

        [SerializeField]
        private float gravity, sensitivity, deadZone;

        public KeyCode Positive { get => positive; set => positive = value; }
        public KeyCode Negative { get => negative; set => negative = value; }

        protected override float UpdateAxisValue()
        {
            float newValue = 0;

            if (Input.GetKey(Positive))
                newValue = AxisValue + (sensitivity * Time.deltaTime);
            else if (Input.GetKey(Negative))
                newValue = AxisValue - (sensitivity * Time.deltaTime);
            else
            {
                if (AxisValue > 0)
                    if (AxisValue < deadZone)
                        newValue = 0;
                    else
                    {
                        newValue = AxisValue - (gravity * Time.deltaTime);
                        if (AxisValue < deadZone)
                            newValue = 0;
                    }
                else if (AxisValue < 0)
                    if (AxisValue > -deadZone)
                        newValue = 0;
                    else
                    {
                        newValue = AxisValue + (gravity * Time.deltaTime);
                        if (AxisValue > -deadZone)
                            newValue = 0;
                    }
            }

            if (newValue > 1)
                newValue = 1;
            else if (newValue < -1)
                newValue = -1;

            return newValue;
        }

        private void OnValidate()
        {
            if (deadZone < 0)
                deadZone = 0;
            else if (deadZone > 1)
                deadZone = 1;
        }
    }
}