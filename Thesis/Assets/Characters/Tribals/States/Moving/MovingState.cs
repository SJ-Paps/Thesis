using SJ.Updatables;
using System;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class MovingState : TribalState, IUpdatable
    {
        private const float VelocityDeadZone = 0.0002f;

        private bool shouldMove;
        private Character.FaceDirection moveDirection;
        private float moveForceMultiplier;

        private bool isFirstUpdate;

        protected override void OnEnter()
        {
            Owner.SubscribeToFixedUpdate(this);

            isFirstUpdate = true;
        }

        protected override void OnUpdate()
        {
            if(isFirstUpdate)
            {
                isFirstUpdate = false;
                Move();
                return;
            }

            if (IsInVelocityDeadZone())
                Trigger(Tribal.Trigger.Stop);
        }

        protected override void OnExit()
        {
            shouldMove = false;

            Owner.UnsubscribeFromFixedUpdate(this);
        }

        protected override bool OnHandleEvent(Character.Order ev)
        {
            switch(ev.type)
            {
                case Character.OrderType.Move:
                    moveDirection = ev.weight >= 0 ? Character.FaceDirection.Right : Character.FaceDirection.Left;
                    moveForceMultiplier = Math.Abs(ev.weight);

                    shouldMove = true;
                    return true;
            }

            
            return false;
        }

        public void DoFixedUpdate()
        {
            if (shouldMove)
                Move();

            shouldMove = false;
        }

        private bool IsInVelocityDeadZone()
        {
            return Owner.CurrentVelocity.x > (VelocityDeadZone * -1) && Owner.CurrentVelocity.x < VelocityDeadZone;
        }
        
        private void Move()
        {
            Owner.Move(moveDirection, moveForceMultiplier);
        }

        public void DoLateUpdate() { }

        public void DoUpdate() { }
    }
}