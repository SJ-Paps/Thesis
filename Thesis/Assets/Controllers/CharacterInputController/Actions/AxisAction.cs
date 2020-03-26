using UnityEngine;

namespace SJ.GameInput
{
    [CreateAssetMenu(menuName = "SJ/Input Actions/Axis Action")]
    public class AxisAction : InputAction, ISerializationCallbackReceiver
    {
        [SerializeField]
        private KeyCode[] defaultPositives, defaultNegatives;

        [SerializeField]
        private float gravity, sensitivity, deadZone;

        [SerializeField]
        private bool snap;

        public float AxisValue { get; private set; }
        public float Gravity { get; set; }
        public float Sensitivity { get; set; }
        public float DeadZone { get; set; }

        public bool Snap { get; set; }

        private KeyCode[] Positives { get; set; } = new KeyCode[0];
        private KeyCode[] Negatives { get; set; } = new KeyCode[0];

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            SetPositives(defaultPositives);
            SetNegatives(defaultNegatives);

            Gravity = gravity;
            Sensitivity = sensitivity;
            DeadZone = deadZone;
            Snap = snap;
        }

        public void SetPositives(params KeyCode[] positives)
        {
            Positives = new KeyCode[positives.Length];
            positives.CopyTo(Positives, 0);
        }

        public void SetNegatives(params KeyCode[] negatives)
        {
            Negatives = new KeyCode[negatives.Length];
            negatives.CopyTo(Negatives, 0);
        }

        public void UpdateStatus()
        {
            AxisValue = UpdateAxisValue();
        }

        public override bool WasTriggeredThisFrame()
        {
            return AxisValue != 0;
        }

        private float UpdateAxisValue()
        {
            float newValue = 0;

            if (AnyPositiveTriggered())
            {
                if (Snap && AxisValue < 0)
                    newValue = (Sensitivity * Time.deltaTime);
                else
                    newValue = AxisValue + (Sensitivity * Time.deltaTime);
            }
            else if (AnyNegativeTriggered())
            {
                if(Snap && AxisValue > 0)
                    newValue = (Sensitivity * Time.deltaTime) * -1;
                else
                    newValue = AxisValue - (Sensitivity * Time.deltaTime);
            }
            else
            {
                if (AxisValue > 0)
                    if (AxisValue < DeadZone)
                        newValue = 0;
                    else
                    {
                        newValue = AxisValue - (Gravity * Time.deltaTime);
                        if (newValue < DeadZone)
                            newValue = 0;
                    }
                else if (AxisValue < 0)
                    if (AxisValue > -DeadZone)
                        newValue = 0;
                    else
                    {
                        newValue = AxisValue + (Gravity * Time.deltaTime);
                        if (newValue > -DeadZone)
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
            if (DeadZone < 0)
                DeadZone = 0;
            else if (DeadZone > 1)
                DeadZone = 1;
        }

        private bool AnyPositiveTriggered()
        {
            for (int i = 0; i < Positives.Length; i++)
                if (Input.GetKey(Positives[i]))
                    return true;

            return false;
        }

        private bool AnyNegativeTriggered()
        {
            for (int i = 0; i < Negatives.Length; i++)
                if (Input.GetKey(Negatives[i]))
                    return true;

            return false;
        }
    }
}