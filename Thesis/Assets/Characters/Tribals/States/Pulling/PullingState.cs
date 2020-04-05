using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class PullingState : TribalSimpleState
    {
        private IMovableObject movableObject;

        protected override void OnEnter()
        {
            Debug.Log("ENTER PULLING STATE");
            
            movableObject = Blackboard.GetItem<IMovableObject>(Tribal.BlackboardKeys.MovableObject);

            movableObject.Connect(Owner.RigidBody2D);
            movableObject.OnLinkBreak += Release;
        }

        protected override void OnExit()
        {
            Debug.Log("EXIT PULLING STATE");
            movableObject.Disconnect();
            movableObject.OnLinkBreak -= Release;
        }

        protected override bool OnHandleEvent(Character.Order ev)
        {
            if(ev.type == Character.OrderType.Jump)
            {
                return true;
            }

            return false;
        }

        private void Release()
        {
            Debug.Log("LINK BREAK");
            Trigger(Tribal.Trigger.Release);
        }
    }
}