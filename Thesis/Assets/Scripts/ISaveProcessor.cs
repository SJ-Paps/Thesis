namespace SJ.GameEntities
{
    public interface ISaveProcessor
    {
        void Save(object save);
        void Load(object save);
    }
}