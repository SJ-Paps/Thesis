using SJ.GameEntities.Characters;
using SJ;
using SJ.Tools;
using UnityEngine;

namespace SJ.GameEntities
{
    public interface IMovableObject
    {
        void Connect(IRigidbody2D rigidbody2D);
    }

    public class MovableObject : SJMonoBehaviour, IMovableObject
    {
        private new Rigidbody2D rigidbody2D;

        public void Connect(IRigidbody2D rigidbody2D)
        {
            
        }
    }
}
