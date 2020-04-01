using SJ.Management;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class BrakingState : TribalState, IFixedUpdateListener
    {
        [SerializeField]
        private float brakeForce;

        protected override void OnEnter()
        {
            Owner.SubscribeToFixedUpdate(this);
        }

        protected override void OnUpdate()
        {
            if (Owner.RigidBody2D.velocity.x == 0)
                Trigger(Tribal.Trigger.Stop);
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
                    Blackboard.SetItem(Tribal.BlackboardKeys.MovingInitialDirectionAndForce, ev.weight);
                    Trigger(Tribal.Trigger.Trot);
                    return true;
            }

            return false;
        }

        public void Brake(float force)
        {
            if (Owner.RigidBody2D.velocity.x > 0)
            {
                ApplyForceOnDirection(FaceDirection.Left, force);

                if (Owner.RigidBody2D.velocity.x < 0)
                    StopCompletely();
            }
            else if (Owner.RigidBody2D.velocity.x < 0)
            {
                ApplyForceOnDirection(FaceDirection.Right, force);

                if (Owner.RigidBody2D.velocity.x < 0)
                    StopCompletely();
            }
        }

        private void ApplyForceOnDirection(FaceDirection direction, float force)
        {
            Owner.RigidBody2D.AddForce(new Vector2((int)direction * force, 0), ForceMode2D.Impulse);
        }

        public void StopCompletely()
        {
            Owner.RigidBody2D.velocity = Vector2.zero;
        }
    }
}