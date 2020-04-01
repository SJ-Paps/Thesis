using UnityEngine;

namespace SJ.Tools
{
    public interface IOnCollisionExit2DListener
    {
        void DoOnCollisionExit(Collision2D collision);
    }
}