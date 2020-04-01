using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace SJ.Tools
{
    public class Eyes : IList<Eye>
    {
        public event Action<Collider2D, Eye> onAnyEntered;
        public event Action<Collider2D, Eye> onAnyStay;
        public event Action<Collider2D, Eye> onAnyExited;

        private Action<Collider2D, Eye> onEnteredTriggerDelegate;
        private Action<Collider2D, Eye> onStayTriggerDelegate;
        private Action<Collider2D, Eye> onExitedTriggerDelegate;


        private List<Eye> eyes;

        public int Count
        {
            get
            {
                return eyes.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public Eye this[int index]
        {
            get
            {
                return eyes[index];
            }

            set
            {
                eyes[index] = value;
            }
        }

        public Eyes()
        {
            onEnteredTriggerDelegate = OnEnteredTrigger;
            onStayTriggerDelegate = OnStayTrigger;
            onExitedTriggerDelegate = OnExitedTrigger;

            eyes = new List<Eye>();
        }

        public Eyes(IEnumerable<Eye> collection) : this()
        {
            AddRange(collection);
        }

        private void OnEnteredTrigger(Collider2D collider, Eye eye)
        {
            if (onAnyEntered != null)
            {
                onAnyEntered(collider, eye);
            }
        }

        private void OnStayTrigger(Collider2D collider, Eye eye)
        {
            if (onAnyStay != null)
            {
                onAnyStay(collider, eye);
            }
        }

        private void OnExitedTrigger(Collider2D collider, Eye eye)
        {
            if (onAnyExited != null)
            {
                onAnyExited(collider, eye);
            }
        }

        private void Bind(Eye eye)
        {
            if (eye == null)
            {
                throw new ArgumentNullException();
            }

            eye.onEntered += onEnteredTriggerDelegate;
            eye.onStay += onStayTriggerDelegate;
            eye.onExited += onExitedTriggerDelegate;
        }

        private void Clear(Eye eye)
        {
            if (eye == null)
            {
                throw new ArgumentNullException();
            }

            eye.onEntered -= onEnteredTriggerDelegate;
            eye.onStay -= onStayTriggerDelegate;
            eye.onExited -= onExitedTriggerDelegate;
        }

        public IEnumerator<Eye> GetEnumerator()
        {
            return eyes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Eye item)
        {
            Bind(item);

            eyes.Add(item);
        }

        public void AddRange(IEnumerable<Eye> collection)
        {
            foreach (Eye eye in collection)
            {
                Bind(eye);
            }

            eyes.AddRange(collection);
        }

        public void Clear()
        {
            for (int i = 0; i < eyes.Count; i++)
            {
                Clear(eyes[i]);
            }

            eyes.Clear();
        }

        public bool Contains(Eye item)
        {
            return eyes.Contains(item);
        }

        public void CopyTo(Eye[] array, int arrayIndex)
        {
            eyes.CopyTo(array, arrayIndex);
        }

        public bool Remove(Eye item)
        {
            if (eyes.Remove(item))
            {
                Clear(item);
                return true;
            }

            return false;
        }

        public int IndexOf(Eye item)
        {
            return eyes.IndexOf(item);
        }

        public void Insert(int index, Eye item)
        {
            Bind(item);

            eyes.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Remove(eyes[index]);
        }
    }
}