using UnityEngine;

namespace SJ.Tools
{
    public interface IOnCollisionEnter2DListener
    {
        void DoOnCollisionEnter(Collision2D collision);
    }
}