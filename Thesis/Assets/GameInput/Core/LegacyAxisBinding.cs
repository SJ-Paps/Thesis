using UnityEngine;

namespace SJ.GameInput
{
    [CreateAssetMenu(menuName = "SJ/Game Input/Input Bindings/Legacy Axis Binding")]
    public class LegacyAxisBinding : AxisBinding
    {
        [SerializeField]
        private string axisName;

        protected override float UpdateAxisValue()
        {
            return Input.GetAxis(axisName);
        }
    }
}