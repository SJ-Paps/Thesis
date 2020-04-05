namespace SJ.Management
{
    public interface IGameEntity : ICompositeUpdateable
    {
        string EntityGUID { get; }
    }
}