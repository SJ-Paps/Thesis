using System;
using System.Collections.Generic;

namespace SJ.Tools
{
    public sealed class Blackboard : IBlackboard
    {
        private Dictionary<string, BlackboardItem> items = new Dictionary<string, BlackboardItem>();
        private Dictionary<string, Action> valueChangedCallbacks = new Dictionary<string, Action>();

        public T GetItem<T>(string identifier)
        {
            var item = (BlackboardItem<T>)items[identifier];

            return item.GetValue();
        }

        public bool TryGetItem<T>(string identifier, out T value)
        {
            if (ContainsItem(identifier))
            {
                value = GetItem<T>(identifier);
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public bool RemoveItem(string identifier)
        {
            if (items.Remove(identifier))
            {
                valueChangedCallbacks.Remove(identifier);

                return true;
            }
            
            return false;
        }

        public void SetItem<T>(string identifier, T value)
        {
            if (items.ContainsKey(identifier) == false)
            {
                items.Add(identifier, new BlackboardItem<T>(value));
            }
            else
            {
                var item = (BlackboardItem<T>)items[identifier];
                item.SetValue(value);
                
                CallOnValueChangedCallbacksFor(identifier);
            }
        }

        private void CallOnValueChangedCallbacksFor(string identifier)
        {
            if(valueChangedCallbacks.ContainsKey(identifier))
                valueChangedCallbacks[identifier].Invoke();
        }

        public bool ContainsItem(string identifier) => items.ContainsKey(identifier);
        public void AddOnValueChangedListener(string identifier, Action callback)
        {
            if(ContainsItem(identifier) == false)
                return;
            
            if (valueChangedCallbacks.ContainsKey(identifier) == false)
                valueChangedCallbacks.Add(identifier, callback);
            else
                valueChangedCallbacks[identifier] += callback;
        }

        public void RemoveOnValueChangedListener(string identifier, Action callback)
        {
            if (valueChangedCallbacks.ContainsKey(identifier))
            {
                valueChangedCallbacks[identifier] -= callback;

                if (valueChangedCallbacks[identifier] == null)
                    valueChangedCallbacks.Remove(identifier);
            }
        }
    }

}