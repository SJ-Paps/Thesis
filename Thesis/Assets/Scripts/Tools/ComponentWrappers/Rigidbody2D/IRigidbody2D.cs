using UnityEngine;

namespace SJ.Tools
{
    public interface IRigidbody2D
    {
        Rigidbody2D InnerComponent { get; }

        Vector2 Velocity { get; set; }
        float Drag { get; set; }
        RigidbodyConstraints2D Constraints { get; set; }

        void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Force);
    }
}