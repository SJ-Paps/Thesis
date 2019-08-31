using UnityEngine;

public class MainCharacter : Tribal
{

    protected override void SJAwake()
    {
        base.SJAwake();

        gameObject.layer = Reg.playerLayer;
    }

    public override void GetEnslaved()
    {
        enslaved = true;
    }
}
