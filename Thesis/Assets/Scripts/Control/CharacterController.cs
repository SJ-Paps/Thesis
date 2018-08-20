using UnityEngine;

public class CharacterController : UnityController<Character> {

    public CharacterController(string name, Character slave) : base(name)
    {
        SetSlave(slave);
    }

    public override void Control()
    {
        if(Input.GetKey(KeyCode.A))
        {
            slave.SetOrder(Character.Order.OrderMoveLeft);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            slave.SetOrder(Character.Order.OrderMoveRight);
        }

        if(Input.GetKey(KeyCode.Space))
        {
            slave.SetOrder(Character.Order.OrderJump);
        }
    }
}
