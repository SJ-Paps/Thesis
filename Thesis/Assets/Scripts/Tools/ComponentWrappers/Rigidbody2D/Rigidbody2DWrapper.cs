using UnityEngine;

namespace SJ.Tools
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rigidbody2DWrapper : SJMonoBehaviour, IRigidbody2D
    {
        private new Rigidbody2D rigidbody2D;

        private Rigidbody2D Rigidbody2D
        {
            get
            {
                if(rigidbody2D == null)
                    rigidbody2D = GetComponent<Rigidbody2D>();

                return rigidbody2D;
            }
        }

        public Vector2 velocity { get => Rigidbody2D.velocity; set => Rigidbody2D.velocity = value; }

        public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
        {
            Rigidbody2D.AddForce(force, mode);
        }
    }
}