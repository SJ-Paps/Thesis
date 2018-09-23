using SAM.FSM;
using System;
using UnityEngine;

public class XenophobicAlertlessState : XenophobicIAState {

    private Vector2 distantVisionSize;
    private float distantVisionOffsetX;

    private Action<Collider2D> onSomethingDetectedDelegate;
    private Eyes characterEyes;

	public XenophobicAlertlessState(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard) : base(fsm, state, controller, blackboard)
    {
        onSomethingDetectedDelegate = GetAware;

        characterEyes = controller.SlaveEyes;


        distantVisionSize = characterEyes.DistantVision.InnerCollider.size;
        distantVisionOffsetX = characterEyes.DistantVision.InnerCollider.offset.x;
    }

    protected override void OnEnter()
    {
        if(characterEyes != null)
        {
            characterEyes.DistantVision.InnerCollider.size = distantVisionSize;
            characterEyes.DistantVision.InnerCollider.offset = new Vector2(distantVisionOffsetX, characterEyes.DistantVision.InnerCollider.offset.y);

            characterEyes.onDistantVisionEnter += onSomethingDetectedDelegate;
            characterEyes.onMediumVisionEnter += onSomethingDetectedDelegate;
            characterEyes.onNearVisionEnter += onSomethingDetectedDelegate;
        }
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnExit()
    {
        if (characterEyes != null)
        {
            characterEyes.onDistantVisionEnter -= onSomethingDetectedDelegate;
            characterEyes.onMediumVisionEnter -= onSomethingDetectedDelegate;
            characterEyes.onNearVisionEnter -= onSomethingDetectedDelegate;
        }
    }

    private void GetAware(Collider2D collider)
    {
        blackboard.seekedLastPosition = collider.transform.position;
        stateMachine.Trigger(XenophobicIAController.Trigger.GetAware);
    }
}
