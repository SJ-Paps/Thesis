using SJ.Updatables;
using System;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class MovingState : TribalState, IUpdatable
    {
        private bool shouldMove;
        private Character.FaceDirection moveDirection;
        private float moveForceMultiplier;
        private bool isMovingByWill;

        protected override void OnEnter()
        {
            Owner.SubscribeToFixedUpdate(this);

            isMovingByWill = true;
        }

        protected override void OnUpdate()
        {
            if (isMovingByWill == false)
                Trigger(Tribal.Trigger.Stop);
                
            isMovingByWill = false;
        }

        protected override void OnExit()
        {
            shouldMove = false;
            isMovingByWill = false;

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
                    isMovingByWill = true;
                    return true;

                case Character.OrderType.Run:
                    Trigger(Tribal.Trigger.Run);
                    return true;

                case Character.OrderType.Walk:
                    Trigger(Tribal.Trigger.Walk);
                    return true;
            }
            
            return false;
        }

        public void DoFixedUpdate()
        {
            if (shouldMove)
            {
                Owner.Face(moveDirection);
                Move();
            }

            shouldMove = false;
        }
        
        private void Move()
        {
            Owner.Move(moveDirection, moveForceMultiplier);
        }

        public void DoLateUpdate() { }

        public void DoUpdate() { }
    }
}