public class MainCharacter : Tribal
{
    protected override void Awake()
    {
        base.Awake();

        gameObject.layer = Reg.playerLayer;

        GameManager.GetInstance().GetPlayer();
    }

    public override void GetEnslaved()
    {
        enslaved = true;
    }

}
