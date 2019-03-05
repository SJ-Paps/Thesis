using UnityEngine;

public class TribalCheckIsValidLadderPosition : TribalGuardCondition
{
    protected override bool OnValidate()
    {
        if(Blackboard.activable is Ladder ladder)
        {
            return ladder.Collider.OverlapPoint((Vector2)Owner.transform.position + Owner.Collider.offset);
        }

        return false;
    }
}
