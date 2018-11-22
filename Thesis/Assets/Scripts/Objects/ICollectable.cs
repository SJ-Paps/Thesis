public interface ICollectable
{
    Character User { get; }

    bool Collect(Character user);

    bool Drop();
}
