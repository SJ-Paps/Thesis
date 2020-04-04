using System.Collections.Generic;
using UnityEngine;

namespace SJ.Tools
{
    public class CompositeCollisionTrigger2DCallbackCaller : SJMonoBehaviour,
        ICompositeCollisionEnter2DCallbackCaller, ICompositeCollisionStay2DCallbackCaller, 
        ICompositeCollisionExit2DCallbackCaller, ICompositeTriggerEnter2DCallbackCaller, 
        ICompositeTriggerStay2DCallbackCaller, ICompositeTriggerExit2DCallbackCaller
    {
        public bool DestroyCallbacksWhenThereIsNoListener;

        private int collisionEnterCurrentIndex = 0;
        private int collisionStayCurrentIndex = 0;
        private int collisionExitCurrentIndex = 0;
        private int triggerEnterCurrentIndex = 0;
        private int triggerStayCurrentIndex = 0;
        private int triggerExitCurrentIndex = 0;

        private List<IOnCollisionEnter2DListener> collisionEnterListeners = new List<IOnCollisionEnter2DListener>();
        private List<IOnCollisionStay2DListener> collisionStayListeners = new List<IOnCollisionStay2DListener>();
        private List<IOnCollisionExit2DListener> collisionExitListeners = new List<IOnCollisionExit2DListener>();
        private List<IOnTriggerEnter2DListener> triggerEnterListeners = new List<IOnTriggerEnter2DListener>();
        private List<IOnTriggerStay2DListener> triggerStayListeners = new List<IOnTriggerStay2DListener>();
        private List<IOnTriggerExit2DListener> triggerExitListeners = new List<IOnTriggerExit2DListener>();

        private CollisionEnterCallback collisionEnterCallback;
        private CollisionStayCallback collisionStayCallback;
        private CollisionExitCallback collisionExitCallback;
        private TriggerEnterCallback triggerEnterCallback;
        private TriggerStayCallback triggerStayCallback;
        private TriggerExitCallback triggerExitCallback;

        public void SubscribeToOnCollisionEnter(IOnCollisionEnter2DListener listener)
        {
            if (collisionEnterCallback == null)
            {
                collisionEnterCallback = gameObject.AddComponent<CollisionEnterCallback>();
                collisionEnterCallback.Set(this);
            }

            if(collisionEnterListeners.Contains(listener) == false)
                collisionEnterListeners.Add(listener);
        }

        public void SubscribeToOnCollisionExit(IOnCollisionExit2DListener listener)
        {
            if(collisionExitCallback == null)
            {
                collisionExitCallback = gameObject.AddComponent<CollisionExitCallback>();
                collisionExitCallback.Set(this);
            }

            if (collisionExitListeners.Contains(listener) == false)
                collisionExitListeners.Add(listener);
        }

        public void SubscribeToOnCollisionStay(IOnCollisionStay2DListener listener)
        {
            if(collisionStayCallback == null)
            {
                collisionStayCallback = gameObject.AddComponent<CollisionStayCallback>();
                collisionStayCallback.Set(this);
            }

            if (collisionStayListeners.Contains(listener) == false)
                collisionStayListeners.Add(listener);
        }

        public void SubscribeToOnTriggerEnter(IOnTriggerEnter2DListener listener)
        {
            if(triggerEnterCallback == null)
            {
                triggerEnterCallback = gameObject.AddComponent<TriggerEnterCallback>();
                triggerEnterCallback.Set(this);
            }

            if (triggerEnterListeners.Contains(listener) == false)
                triggerEnterListeners.Add(listener);
        }

        public void SubscribeToOnTriggerExit(IOnTriggerExit2DListener listener)
        {
            if(triggerExitCallback == null)
            {
                triggerExitCallback = gameObject.AddComponent<TriggerExitCallback>();
                triggerExitCallback.Set(this);
            }

            if (triggerExitListeners.Contains(listener) == false)
                triggerExitListeners.Add(listener);
        }

        public void SubscribeToOnTriggerStay(IOnTriggerStay2DListener listener)
        {
            if(triggerStayCallback == null)
            {
                triggerStayCallback = gameObject.AddComponent<TriggerStayCallback>();
                triggerStayCallback.Set(this);
            }

            if (triggerStayListeners.Contains(listener) == false)
                triggerStayListeners.Add(listener);
        }

        public void UnsubscribeFromOnCollisionEnter(IOnCollisionEnter2DListener listener)
        {
            collisionEnterListeners.Remove(listener);

            if (collisionEnterCurrentIndex > 0)
                collisionEnterCurrentIndex--;

            if(DestroyCallbacksWhenThereIsNoListener && collisionEnterListeners.Count == 0)
            {
                Destroy(collisionEnterCallback);
                collisionEnterCallback = null;
            }
        }

        public void UnsubscribeFromOnCollisionExit(IOnCollisionExit2DListener listener)
        {
            collisionExitListeners.Remove(listener);

            if (collisionExitCurrentIndex > 0)
                collisionExitCurrentIndex--;

            if (DestroyCallbacksWhenThereIsNoListener && collisionExitListeners.Count == 0)
            {
                Destroy(collisionExitCallback);
                collisionExitCallback = null;
            }
        }

        public void UnsubscribeFromOnCollisionStay(IOnCollisionStay2DListener listener)
        {
            collisionStayListeners.Remove(listener);

            if (collisionStayCurrentIndex > 0)
                collisionStayCurrentIndex--;

            if (DestroyCallbacksWhenThereIsNoListener && collisionStayListeners.Count == 0)
            {
                Destroy(collisionStayCallback);
                collisionStayCallback = null;
            }
        }

        public void UnsubscribeFromOnTriggerEnter(IOnTriggerEnter2DListener listener)
        {
            triggerEnterListeners.Remove(listener);

            if (triggerEnterCurrentIndex > 0)
                triggerEnterCurrentIndex--;

            if(DestroyCallbacksWhenThereIsNoListener && triggerEnterListeners.Count == 0)
            {
                Destroy(triggerEnterCallback);
                triggerEnterCallback = null;
            }
        }

        public void UnsubscribeFromOnTriggerExit(IOnTriggerExit2DListener listener)
        {
            triggerExitListeners.Remove(listener);

            if (triggerExitCurrentIndex > 0)
                triggerExitCurrentIndex--;

            if (DestroyCallbacksWhenThereIsNoListener && triggerExitListeners.Count == 0)
            {
                Destroy(triggerExitCallback);
                triggerExitCallback = null;
            }
        }

        public void UnsubscribeFromOnTriggerStay(IOnTriggerStay2DListener listener)
        {
            triggerStayListeners.Remove(listener);

            if (triggerStayCurrentIndex > 0)
                triggerStayCurrentIndex--;

            if (DestroyCallbacksWhenThereIsNoListener && triggerStayListeners.Count == 0)
            {
                Destroy(triggerStayCallback);
                triggerStayCallback = null;
            }
        }

        private void CallOnCollisionEnter(Collision2D collision)
        {
            for (collisionEnterCurrentIndex = 0; collisionEnterCurrentIndex < collisionEnterListeners.Count; collisionEnterCurrentIndex++)
                collisionEnterListeners[collisionEnterCurrentIndex].DoOnCollisionEnter(collision);
        }

        private void CallOnCollisionStay(Collision2D collision)
        {
            for (collisionStayCurrentIndex = 0; collisionStayCurrentIndex < collisionStayListeners.Count; collisionStayCurrentIndex++)
                collisionStayListeners[collisionStayCurrentIndex].DoOnCollisionStay(collision);
        }

        private void CallOnCollisionExit(Collision2D collision)
        {
            for (collisionExitCurrentIndex = 0; collisionExitCurrentIndex < collisionExitListeners.Count; collisionExitCurrentIndex++)
                collisionExitListeners[collisionExitCurrentIndex].DoOnCollisionExit(collision);
        }

        private void CallOnTriggerEnter(Collider2D collider)
        {
            for (triggerEnterCurrentIndex = 0; triggerEnterCurrentIndex < triggerEnterListeners.Count; triggerEnterCurrentIndex++)
                triggerEnterListeners[triggerEnterCurrentIndex].DoOnTriggerEnter(collider);
        }

        private void CallOnTriggerStay(Collider2D collider)
        {
            for (triggerStayCurrentIndex = 0; triggerStayCurrentIndex < triggerStayListeners.Count; triggerStayCurrentIndex++)
                triggerStayListeners[triggerStayCurrentIndex].DoOnTriggerStay(collider);
        }

        private void CallOnTriggerExit(Collider2D collider)
        {
            for (triggerExitCurrentIndex = 0; triggerExitCurrentIndex < triggerExitListeners.Count; triggerExitCurrentIndex++)
                triggerExitListeners[triggerExitCurrentIndex].DoOnTriggerExit(collider);
        }

        private class CollisionEnterCallback : SJMonoBehaviour
        {
            private CompositeCollisionTrigger2DCallbackCaller collidable;

            public void Set(CompositeCollisionTrigger2DCallbackCaller collidable)
            {
                this.collidable = collidable;
            }

            private void OnCollisionEnter2D(Collision2D collision)
            {
                collidable.CallOnCollisionEnter(collision);
            }
        }

        private class CollisionStayCallback : SJMonoBehaviour
        {
            private CompositeCollisionTrigger2DCallbackCaller collidable;

            public void Set(CompositeCollisionTrigger2DCallbackCaller collidable)
            {
                this.collidable = collidable;
            }

            private void OnCollisionStay2D(Collision2D collision)
            {
                collidable.CallOnCollisionStay(collision);
            }
        }

        private class CollisionExitCallback : SJMonoBehaviour
        {
            private CompositeCollisionTrigger2DCallbackCaller collidable;

            public void Set(CompositeCollisionTrigger2DCallbackCaller collidable)
            {
                this.collidable = collidable;
            }

            private void OnCollisionExit2D(Collision2D collision)
            {
                collidable.CallOnCollisionExit(collision);
            }
        }

        private class TriggerEnterCallback : SJMonoBehaviour
        {
            private CompositeCollisionTrigger2DCallbackCaller collidable;

            public void Set(CompositeCollisionTrigger2DCallbackCaller collidable)
            {
                this.collidable = collidable;
            }

            private void OnTriggerEnter2D(Collider2D collider)
            {
                collidable.CallOnTriggerEnter(collider);
            }
        }

        private class TriggerStayCallback : SJMonoBehaviour
        {
            private CompositeCollisionTrigger2DCallbackCaller collidable;

            public void Set(CompositeCollisionTrigger2DCallbackCaller collidable)
            {
                this.collidable = collidable;
            }

            private void OnTriggerStay2D(Collider2D collider)
            {
                collidable.CallOnTriggerStay(collider);
            }
        }

        private class TriggerExitCallback : SJMonoBehaviour
        {
            private CompositeCollisionTrigger2DCallbackCaller collidable;

            public void Set(CompositeCollisionTrigger2DCallbackCaller collidable)
            {
                this.collidable = collidable;
            }

            private void OnTriggerExit2D(Collider2D collider)
            {
                collidable.CallOnTriggerExit(collider);
            }
        }
    }
}