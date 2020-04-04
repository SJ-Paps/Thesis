using UnityEngine;

namespace SJ.Tools
{
    public interface IRigidbody2D
    {
        Rigidbody2D InnerComponent { get; }

        Vector2 velocity { get; set; }

        void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force);
    }
}