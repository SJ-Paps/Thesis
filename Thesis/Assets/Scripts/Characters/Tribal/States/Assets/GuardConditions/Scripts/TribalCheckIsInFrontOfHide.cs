using UnityEngine;

public class TribalCheckIsInFrontOfHide : TribalGuardCondition
{
    protected override bool Validate()
    {
        if(BlackboardContainsHide())
        {
            return true;
        }

        return FindHide();
    }

    private bool FindHide()
    {
        Vector2 detectionSize = new Vector2((Owner.Collider.bounds.extents.x * 2) + Tribal.activableDetectionOffset, Owner.Collider.bounds.extents.y * 2);

        Hide hide = SJUtil.FindActivable<Hide, Character>(Owner.Collider.bounds.center, detectionSize, Owner.transform.eulerAngles.z);

        if (hide != null)
        {

            Blackboard.CurrentFrameActivables.Add(hide);
            return true;
        }

        return false;
    }

    private bool BlackboardContainsHide()
    {
        return Blackboard.CurrentFrameActivables.ContainsType<Hide>();
    }
}
