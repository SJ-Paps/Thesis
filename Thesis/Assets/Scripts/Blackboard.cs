using System.Collections.Generic;

public sealed class Blackboard : IBlackboard
{
    private Dictionary<string, BlackboardItem> items;

    public Blackboard()
    {
        items = new Dictionary<string, BlackboardItem>();
    }

    public T GetItemOf<T>(string identifier)
    {
        var item = (BlackboardItem<T>)items[identifier];

        return item.GetValue();
    }

    public bool RemoveItem(string identifier)
    {
        return items.Remove(identifier);
    }

    public void SetItem<T>(string identifier, T value)
    {
        if (items.ContainsKey(identifier) == false)
            items.Add(identifier, new BlackboardItem<T>(value));

        var item = (BlackboardItem<T>)items[identifier];

        item.SetValue(value);
    }
}
