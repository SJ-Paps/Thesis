using UnityEngine;

namespace SJ.GameInput
{
    [CreateAssetMenu(menuName = "SJ/Game Input/Input Bindings/Custom Axis Binding")]
    public class CustomAxisBinding : AxisBinding, ISerializationCallbackReceiver
    {
        [SerializeField]
        private KeyCode positive, negative;

        [SerializeField]
        private float gravity, sensitivity, deadZone;

        [SerializeField]
        private bool snap;

        public KeyCode Positive { get; set; }
        public KeyCode Negative { get; set; }

        public void OnAfterDeserialize()
        {
            Positive = positive;
            Negative = negative;
        }

        public void OnBeforeSerialize()
        {
            
        }

        protected override float UpdateAxisValue()
        {
            float newValue = 0;

            if (Input.GetKey(Positive))
            {
                if (snap && AxisValue < 0)
                    newValue = (sensitivity * Time.deltaTime);
                else
                    newValue = AxisValue + (sensitivity * Time.deltaTime);
            }
            else if (Input.GetKey(Negative))
            {
                if(snap && AxisValue > 0)
                    newValue = (sensitivity * Time.deltaTime) * -1;
                else
                    newValue = AxisValue - (sensitivity * Time.deltaTime);
            }
            else
            {
                if (AxisValue > 0)
                    if (AxisValue < deadZone)
                        newValue = 0;
                    else
                    {
                        newValue = AxisValue - (gravity * Time.deltaTime);
                        if (newValue < deadZone)
                            newValue = 0;
                    }
                else if (AxisValue < 0)
                    if (AxisValue > -deadZone)
                        newValue = 0;
                    else
                    {
                        newValue = AxisValue + (gravity * Time.deltaTime);
                        if (newValue > -deadZone)
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