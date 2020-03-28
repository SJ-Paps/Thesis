using System;

namespace SJ.Tools
{
    public class BlackboardItem<T> : BlackboardItem
    {
        public event Action<T> OnValueChanged;

        private T value;

        public BlackboardItem(T value)
        {
            this.value = value;
        }

        public void SetValue(T value)
        {
            this.value = value;

            OnValueChanged?.Invoke(this.value);
        }

        public T GetValue()
        {
            return value;
        }

        public override object GetValueObject()
        {
            return value;
        }
    }
}