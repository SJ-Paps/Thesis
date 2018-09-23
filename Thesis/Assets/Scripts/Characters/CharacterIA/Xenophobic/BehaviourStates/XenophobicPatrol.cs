using SAM.FSM;
using UnityEngine;

public class XenophobicPatrol : XenophobicIAState
{
    private float xMargin = 4f;

    private float worldLeftXMargin;
    private float worldRightXMargin;

    private Character.Order currentOrder;

    public XenophobicPatrol(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state, controller, blackboard)
    {

    }

    protected override void OnEnter()
    {
        controller.Slave.onCollisionEnter2D += CheckCollision;

        currentOrder = Character.Order.OrderMoveLeft;

        worldLeftXMargin = controller.Slave.transform.position.x - xMargin;
        worldRightXMargin = controller.Slave.transform.position.x + xMargin;
    }

    protected override void OnExit()
    {
        controller.Slave.onCollisionEnter2D -= CheckCollision;
    }

    protected override void OnUpdate()
    {
        if(currentOrder == Character.Order.OrderMoveLeft && controller.Slave.transform.position.x < worldLeftXMargin)
        {
            currentOrder = Character.Order.OrderMoveRight;
        }
        else if(currentOrder == Character.Order.OrderMoveRight && controller.Slave.transform.position.x > worldRightXMargin)
        {
            currentOrder = Character.Order.OrderMoveLeft;
        }

        controller.Slave.SetOrder(currentOrder);
    }

    private void CheckCollision(Collision2D collision)
    {
        foreach(ContactPoint2D contact in collision.contacts)
        {
            bool validatedCollision = false;

            if(contact.collider.gameObject.layer == Reg.wallLayer||
                contact.collider.gameObject.layer == Reg.objectLayer)
            {
                validatedCollision = true;
            }

            if(validatedCollision)
            {
                if(contact.normal.x == Vector2.right.x && currentOrder == Character.Order.OrderMoveLeft)
                {
                    currentOrder = Character.Order.OrderMoveRight;
                }
                else if(contact.normal.x == Vector2.left.x && currentOrder == Character.Order.OrderMoveRight)
                {
                    currentOrder = Character.Order.OrderMoveLeft;
                }
            }
        }
    }
}
