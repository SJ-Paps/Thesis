using UnityEngine;

public class TribalHiddenState : TribalHSMState
{
    private Hide currentHide;

    public TribalHiddenState(Character.State state, string debugName) : base(state, debugName)
    {

    }

    protected override void OnEnter() 
    {
        base.OnEnter();

        currentHide = blackboard.toHidePlace;

        if(currentHide == null)
        {
            Vector2 detectionSize = new Vector2((character.Collider.bounds.extents.x * 2) + Tribal.activableDetectionOffset, character.Collider.bounds.extents.y * 2);

            currentHide = SJUtil.FindActivable<Hide, Character>(character.Collider.bounds.center, detectionSize, character.transform.eulerAngles.z);
        }

        if(currentHide == null)
        {
            SendEvent(Character.Trigger.StopHiding);
        }
        else
        {
            currentHide.Activate(character);

            character.Animator.SetTrigger(Tribal.HideAnimatorTrigger);
        }
    }

    protected override void OnExit()
    {
        base.OnExit();

        if(currentHide != null)
        {
            currentHide.Activate(character);

            currentHide = null;

            character.Animator.ResetTrigger(Tribal.HideAnimatorTrigger);
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