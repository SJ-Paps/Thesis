public abstract class GuardCondition
{
    public bool invert;

    public bool IsValid()
    {
        return Validate() ^ invert; //Exclusive OR operation
    }

    protected abstract bool Validate();
}
