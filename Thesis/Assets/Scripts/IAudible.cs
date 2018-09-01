public struct AudibleData
{

}

public interface IAudible
{
    AudibleData MakeNoise();
}

public interface IAudibleListener
{
    void Listen(ref AudibleData data);
}