using UnityEngine;

namespace SJ.Tools
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class CapsuleCollider2DWrapper : SJMonoBehaviour, ICapsuleCollider2D
    {
        private new CapsuleCollider2D collider;

        private CapsuleCollider2D Collider
        {
            get
            {
                if(collider == null)
                    collider = GetComponent<CapsuleCollider2D>();

                return collider;
            }
        }

        public Bounds bounds { get => Collider.bounds; }
    }
}