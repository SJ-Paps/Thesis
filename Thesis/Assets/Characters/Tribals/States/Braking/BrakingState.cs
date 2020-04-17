using SJ.Management;
using SJ.Tools;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class BrakingState : TribalSimpleState, IFixedUpdateListener
    {
        [SerializeField]
        private float brakeForce, velocityDeadZone;

        protected override void OnEnter()
        {
            Owner.SubscribeToFixedUpdate(this);
        }

        protected override void OnUpdate()
        {
            if (Owner.RigidBody2D.Velocity.x > velocityDeadZone * 1 || Owner.RigidBody2D.Velocity.x < velocityDeadZone)
            {
                StopCompletely();
                Trigger(Tribal.Trigger.Stop);
            }  
        }

        protected override void OnExit()
        {
            Owner.UnsubscribeFromFixedUpdate(this);
        }

        public void DoFixedUpdate()
        {
            Brake(brakeForce);
        }

        protected override bool OnHandleEvent(Character.Order ev)
        {
            switch(ev.type)
            {
                case Character.OrderType.Move:
                    Trigger(Tribal.Trigger.Trot);
                    return true;
            }

            return false;
        }

        public void Brake(float force)
        {
            if (Owner.RigidBody2D.Velocity.x > 0)
            {
                ApplyForceOnDirection(HorizontalDirection.Left, force);

                if (Owner.RigidBody2D.Velocity.x < 0)
                    StopCompletely();
            }
            else if (Owner.RigidBody2D.Velocity.x < 0)
            {
                ApplyForceOnDirection(HorizontalDirection.Right, force);

                if (Owner.RigidBody2D.Velocity.x < 0)
                    StopCompletely();
            }
        }

        private void ApplyForceOnDirection(HorizontalDirection direction, float force)
        {
            Owner.RigidBody2D.AddForce(new Vector2((int)direction * force, 0), ForceMode2D.Impulse);
        }

        public void StopCompletely()
        {
            Owner.RigidBody2D.Velocity = Vector2.zero;
        }
    }
}