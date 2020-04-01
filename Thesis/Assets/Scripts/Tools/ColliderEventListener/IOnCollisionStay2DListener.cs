using UnityEngine;

namespace SJ.Tools
{
    public interface IOnCollisionStay2DListener
    {
        void DoOnCollisionStay(Collision2D collision);
    }
}