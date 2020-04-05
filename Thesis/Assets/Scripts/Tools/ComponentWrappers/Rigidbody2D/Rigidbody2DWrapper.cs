using UnityEngine;

namespace SJ.Tools
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rigidbody2DWrapper : SJMonoBehaviour, IRigidbody2D
    {
        private new Rigidbody2D internalRigidBody2D;

        private Rigidbody2D Rigidbody2D
        {
            get
            {
                if(internalRigidBody2D == null)
                    internalRigidBody2D = GetComponent<Rigidbody2D>();

                return internalRigidBody2D;
            }
        }

        public Vector2 Velocity { get => Rigidbody2D.velocity; set => Rigidbody2D.velocity = value; }
        public float Drag { get => Rigidbody2D.drag; set => Rigidbody2D.drag = value; }
        public RigidbodyConstraints2D Constraints { get => Rigidbody2D.constraints; set => Rigidbody2D.constraints = value; }

        public Rigidbody2D InnerComponent => Rigidbody2D;

        public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
        {
            Rigidbody2D.AddForce(force, mode);
        }
    }
}