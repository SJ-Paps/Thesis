using UnityEngine;

public class XenophobicIASeekingState : XenophobicIAState
{
    private float positionReachedDeadZoneX = 1f, positionReachedDeadZoneY = 1f;

    private BlackboardNode<Vector2> lastDetectedPositionNode;

    protected override void OnConstructionFinished()
    {
        base.OnConstructionFinished();

        lastDetectedPositionNode = Blackboard.GetItemNodeOf<Vector2>("LastDetectedPosition");
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(ShouldStop())
        {
            Slave.SendOrder(Character.Order.StopMoving);
        }
        else if(HasReachedTarget(lastDetectedPositionNode.GetValue()) == false)
        {
            SearchAtPosition(lastDetectedPositionNode.GetValue());
        }
    }

    private void SearchAtPosition(Vector2 position)
    {
        if (position.x < Slave.transform.position.x)
        {
            Slave.SendOrder(Character.Order.MoveLeft);
        }
        else
        {
            Slave.SendOrder(Character.Order.MoveRight);
        }
    }

    private bool HasReachedTarget(Vector2 targetPosition)
    {
        Bounds b = new Bounds(targetPosition, new Vector2(positionReachedDeadZoneX * 2, positionReachedDeadZoneY * 2));

        if (b.Contains(Slave.transform.position))
        {
            return true;
        }

        return false;
    }

    private bool ShouldStop()
    {
        return Slave.CheckWall() || Slave.CheckFloorAhead() == false;
    }
}
