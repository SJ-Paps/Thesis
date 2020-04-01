using UnityEngine;

namespace SJ.Tools
{
    public interface IOnTriggerEnter2DListener
    {
        void DoOnTriggerEnter(Collider2D collider);
    }
}