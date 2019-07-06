using UnityEngine;

public class TribalHiddenState : TribalHSMState
{
    private Hide currentHide;

    protected override void OnEnter() 
    {
        base.OnEnter();

        currentHide = Blackboard.GetItemOf<IActivable>("Activable") as Hide;

        Blackboard.UpdateItem<IActivable>("Activable", null);

        if (currentHide == null)
        {
            SendEvent(Character.Order.StopHiding);
        }
        else
        {
            currentHide.Activate(Owner);

            Owner.RigidBody2D.velocity = new Vector2(0, 0);
            Owner.RigidBody2D.MovePosition(new Vector2(currentHide.transform.position.x, Owner.transform.position.y));

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

    protected override bool HandleEvent(Character.Order trigger)
    {
        if(trigger == Character.Order.Jump)
        {
            return true;
        }

        return false;
    }
}