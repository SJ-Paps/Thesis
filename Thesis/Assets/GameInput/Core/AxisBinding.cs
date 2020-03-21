using UnityEngine;

namespace SJ.GameInput
{
    public abstract class AxisBinding : InputBinding
    {
        public float AxisValue { get; private set; }

        protected override sealed bool UpdateTriggeredStatus()
        {
            AxisValue = UpdateAxisValue();

            return AxisValue != 0;
        }

        protected abstract float UpdateAxisValue();
    }
}