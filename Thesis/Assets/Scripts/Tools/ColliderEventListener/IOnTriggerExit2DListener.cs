using UnityEngine;

namespace SJ.Tools
{
    public interface IOnTriggerExit2DListener
    {
        void DoOnTriggerExit(Collider2D collider);
    }
}