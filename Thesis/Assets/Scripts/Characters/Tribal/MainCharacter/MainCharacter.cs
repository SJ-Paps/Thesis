using UnityEngine;

public class MainCharacter : Tribal
{

    protected override void Awake()
    {
        base.Awake();

        gameObject.layer = Reg.playerLayer;
    }

    public override void GetEnslaved()
    {
        enslaved = true;
    }
}
