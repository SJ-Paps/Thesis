using UnityEngine;

namespace SJ.Tools
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Rigidbody2DWrapper : SJMonoBehaviour, IRigidbody2D
    {
        private new Rigidbody2D rigidbody2D;

        public Vector2 velocity
        {
            get
            {
                return rigidbody2D.velocity;
            }

            set
            {
                rigidbody2D.velocity = value;
            }
        }

        protected override void SJAwake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force)
        {
            rigidbody2D.AddForce(force, mode);
        }
    }
}