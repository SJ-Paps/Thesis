using SAM.FSM;
using System;
using UnityEngine;

[Serializable]
public class XenophobicAlertlessState : XenophobicIAState {

    [SerializeField]
    private Vector2 eyesSize = new Vector2(9, 1);

    private Action<Collider2D> onSomethingDetectedDelegate;
    private Eyes characterEyes;
    
    private int targetLayers;

    public override void InitializeState(FSM<XenophobicIAController.State, XenophobicIAController.Trigger> fsm, XenophobicIAController.State state, XenophobicIAController controller, XenophobicIAController.Blackboard blackboard)
    {
        base.InitializeState(fsm, state, controller, blackboard);

        onSomethingDetectedDelegate += AnalyzeDetection;

        characterEyes = controller.SlaveEyes;

        targetLayers = (1 << Reg.playerLayer);
    }

    protected override void OnEnter()
    {
        if(characterEyes != null)
        {
            characterEyes.Trigger2D.ChangeSize(eyesSize);

            characterEyes.Trigger2D.onStay += onSomethingDetectedDelegate;
        }
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnExit()
    {
        if (characterEyes != null)
        {
            characterEyes.Trigger2D.onStay -= onSomethingDetectedDelegate;
        }
    }

    private void AnalyzeDetection(Collider2D collider)
    {
        if (characterEyes.IsVisible(collider, Reg.walkableLayerMask, targetLayers))
        {
            if(collider.gameObject.layer == Reg.playerLayer && GameManager.Instance.GetPlayer().IsHidden == false)
            {
                stateMachine.Trigger(XenophobicIAController.Trigger.GetAware);
            }
        }
    }
}
