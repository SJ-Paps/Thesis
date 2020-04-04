using SJ.Tools;
using UnityEngine;

namespace SJ.GameEntities.Characters.Tribals.States
{
    public class FindMovableObjectsWhileMovingStateBehaviour : TribalStateBehaviour
    {
        private SyncTimer checkMovableObjectsTimer = new SyncTimer();

        protected override void Initialize()
        {
            InitializeMovableObjectsTimer();
        }

        private void InitializeMovableObjectsTimer()
        {
            checkMovableObjectsTimer.Interval = 0.2f;
            checkMovableObjectsTimer.Loop = true;
            checkMovableObjectsTimer.OnTick += _ => SearchMovableObjects();
        }

        public override void OnEnter()
        {
            checkMovableObjectsTimer.Start();
        }

        public override void OnUpdate()
        {
            checkMovableObjectsTimer.Update(Time.deltaTime);
        }

        public override void OnExit()
        {
            checkMovableObjectsTimer.Stop();
        }

        private void SearchMovableObjects()
        {
            float checkMovableObjectDistanceX = 0.2f;

            int facingDirection = (int)Owner.FacingDirection;

            var bounds = Owner.Collider.bounds;

            var center = new Vector2(bounds.center.x + (bounds.extents.x * facingDirection), bounds.center.y - bounds.extents.y / 3);
            var size = new Vector2(checkMovableObjectDistanceX, bounds.extents.y);

            var movableObject = SJUtil.FindMovableObject(center, size, Owner.transform.eulerAngles.z);

            if (movableObject != null)
            {
                Blackboard.SetItem(Tribal.BlackboardKeys.MovableObject, movableObject);
                Trigger(Tribal.Trigger.Pull);
            }
        }
    }
}