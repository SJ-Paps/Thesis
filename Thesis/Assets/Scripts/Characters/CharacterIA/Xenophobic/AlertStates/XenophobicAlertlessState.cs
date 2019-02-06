using SAM.FSM;
using System;
using UnityEngine;


public class XenophobicAlertlessState : XenophobicIAState {
    

    private Vector2 eyesSize = new Vector2(9, 1);

    private Action<Collider2D> onSomethingDetectedDelegate;
    private Eyes characterEyes;
    
    private int targetLayers;

    public XenophobicAlertlessState(XenophobicIAController.State state, string debugName) : base(state, debugName)
    {
        //onSomethingDetectedDelegate += AnalyzeDetection;

        characterEyes = controller.SlaveEyes;

        targetLayers = (1 << Reg.playerLayer);
    }

    /*protected override void OnEnter()
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
    }*/
}
