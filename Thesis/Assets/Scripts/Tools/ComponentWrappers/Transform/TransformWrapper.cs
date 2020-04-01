using UnityEngine;

namespace SJ.Tools
{
    public class TransformWrapper : SJMonoBehaviour, ITransform
    {
        public Vector3 position { get => transform.position; set => transform.position = value; }
    }
}