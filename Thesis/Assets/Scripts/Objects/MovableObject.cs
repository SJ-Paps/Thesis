using SJ.Tools;
using UnityEngine;

namespace SJ.GameEntities
{
    public class MovableObject : SJMonoBehaviour, IMovableObject
    {
        private new Rigidbody2D rigidbody2D;

        public void Connect(IRigidbody2D rigidbody2D)
        {
            
        }

        public void Disconnect(IRigidbody2D rigidbody2D)
        {
            
        }
    }
}
