using UnityEngine;

public class TribalHiddenState : TribalHSMState
{
    private Hide currentHide;

    public TribalHiddenState(Character.State state, string debugName) : base(state, debugName)
    {
        activeDebug = true;
    }

    protected override void OnEnter() 
    {
        base.OnEnter();

        currentHide = Blackboard.toHidePlace;

        if(currentHide == null)
        {
            Vector2 detectionSize = new Vector2((Owner.Collider.bounds.extents.x * 2) + Tribal.activableDetectionOffset, Owner.Collider.bounds.extents.y * 2);

            currentHide = SJUtil.FindActivable<Hide, Character>(Owner.Collider.bounds.center, detectionSize, Owner.transform.eulerAngles.z);
        }

        if(currentHide == null)
        {
            SendEvent(Character.Trigger.StopHiding);
        }
        else
        {
            currentHide.Activate(Owner);

            Owner.Animator.SetTrigger(Tribal.HideAnimatorTrigger);
        }
    }

    protected override void OnExit()
    {
        base.OnExit();

        if(currentHide != null)
        {
            currentHide.Activate(Owner);

            currentHide = null;

            Owner.Animator.ResetTrigger(Tribal.HideAnimatorTrigger);
        }
    }

    protected override void OnUpdate() 
    {
        base.OnUpdate();
    }

    protected override bool HandleEvent(Character.Trigger trigger)
    {
        if(trigger == Character.Trigger.Jump)
        {
            return true;
        }

        return false;
    }
}