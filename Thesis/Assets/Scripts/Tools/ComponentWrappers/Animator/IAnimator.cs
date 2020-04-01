namespace SJ.Tools
{
    public interface IAnimator
    {
        void SetTrigger(int id);
        void SetTrigger(string trigger);

        void ResetTrigger(int id);
        void ResetTrigger(string trigger);
    }
}