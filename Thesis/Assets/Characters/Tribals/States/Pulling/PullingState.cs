using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class PullingState : TribalSimpleState
    {
        [SerializeField] 
        private float maxMovementVelocityPercentageModifier;
        
        private IMovableObject movableObject;
        
        private int constraintId;
        
        protected override void OnEnter()
        {
            movableObject = Blackboard.GetItem<IMovableObject>(Tribal.BlackboardKeys.MovableObject);

            movableObject.Connect(Owner.RigidBody2D);
            movableObject.OnLinkBreak += Release;
            
            constraintId = Owner.MaxMovementVelocity.AddConstraint(maxMovementVelocityPercentageModifier);
        }

        protected override void OnExit()
        {
            Owner.MaxMovementVelocity.RemoveConstraint(constraintId);
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
            Logger.LogConsole("LINK BREAK");
            Trigger(Tribal.Trigger.Release);
        }
    }
}