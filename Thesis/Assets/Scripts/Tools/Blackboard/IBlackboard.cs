namespace SJ.Tools
{
    public interface IBlackboard
    {
        void SetItem<T>(string identifier, T value);
        T GetItemOf<T>(string identifier);
        bool TryGetItemOf<T>(string identifier, out T value);
        bool RemoveItem(string identifier);
        bool ContainsItem(string identifier);
    }
}