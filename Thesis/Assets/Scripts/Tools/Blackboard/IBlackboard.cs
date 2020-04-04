namespace SJ.Tools
{
    public interface IBlackboard
    {
        void SetItem<T>(string identifier, T value);
        T GetItem<T>(string identifier);
        bool TryGetItem<T>(string identifier, out T value);
        bool RemoveItem(string identifier);
        bool ContainsItem(string identifier);
    }
}