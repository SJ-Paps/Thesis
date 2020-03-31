namespace SJ.Management
{
    public interface IGameEntity : ICompositeUpdatable
    {
        string EntityGUID { get; }
    }
}