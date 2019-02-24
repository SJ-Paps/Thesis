using UnityEngine;

public class XenophobicIASeekingState : XenophobicIAState
{
    private float positionReachedDeadZoneX = 1f, positionReachedDeadZoneY = 1f;

    public XenophobicIASeekingState(XenophobicIAController.State state, string debugName) : base(state, debugName)
    {

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(ShouldStop())
        {
            Owner.Slave.SetOrder(Character.Trigger.StopMoving);
        }
        else if(HasReachedTarget(Blackboard.LastDetectedPosition) == false)
        {
            SearchAtPosition(Blackboard.LastDetectedPosition);
        }
    }

    private void SearchAtPosition(Vector2 position)
    {
        if (position.x < Owner.Slave.transform.position.x)
        {
            Owner.Slave.SetOrder(Character.Trigger.MoveLeft);
        }
        else
        {
            Owner.Slave.SetOrder(Character.Trigger.MoveRight);
        }
    }

    private bool HasReachedTarget(Vector2 targetPosition)
    {
        Bounds b = new Bounds(targetPosition, new Vector2(positionReachedDeadZoneX * 2, positionReachedDeadZoneY * 2));

        if (b.Contains(Owner.Slave.transform.position))
        {
            return true;
        }

        return false;
    }

    private bool ShouldStop()
    {
        return Owner.Slave.CheckWall() || Owner.Slave.CheckFloorAhead() == false;
    }
}
