using UnityEngine;

namespace SJ.Tools
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class CapsuleCollider2DWrapper : SJMonoBehaviour, ICapsuleCollider2D
    {
        private new CapsuleCollider2D collider;

        public Bounds bounds { get => collider.bounds; }

        protected override void SJAwake()
        {
            collider = GetComponent<CapsuleCollider2D>();
        }
    }
}