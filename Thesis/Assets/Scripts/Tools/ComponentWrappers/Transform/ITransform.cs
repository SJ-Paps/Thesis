using UnityEngine;

namespace SJ.Tools
{
    public interface ITransform
    {
        Vector3 position { get; set; }
        Vector3 eulerAngles { get; set; }
    }
}