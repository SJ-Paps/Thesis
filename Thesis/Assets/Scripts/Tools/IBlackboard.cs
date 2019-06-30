using System;

public interface IBlackboard
{
    void CreateSlot(in string identifier);
    BlackboardNode<T> AddItem<T>(in string identifier, T value);
    BlackboardNode<T> GetItemNodeOf<T>(in string identifier);
    BlackboardNode<T> UpdateItem<T>(in string identifier, T newValue);
    T GetItemOf<T>(in string identifier);
    object GetItemOf(in string identifier);
    void RemoveItem(in string identifier);
    void AddListenerToValueChangedTo<T>(in string identifier, Action<T> listener);
    void RemoveListener<T>(in string identifier, Action<T> listener);
}

public abstract class BlackboardNode
{
    public abstract object GetValueObject();
}

public class BlackboardNode<T> : BlackboardNode
{
    public event Action<T> onValueChanged;

    private T value;

    public BlackboardNode(T value)
    {
        this.value = value;
    }

    public void SetValue(T value)
    {
        this.value = value;

        if(onValueChanged != null)
        {
            onValueChanged(this.value);
        }
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
