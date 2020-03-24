public interface IBlackboard
{
    void SetItem<T>(string identifier, T value);
    T GetItemOf<T>(string identifier);
    bool RemoveItem(string identifier);
}
