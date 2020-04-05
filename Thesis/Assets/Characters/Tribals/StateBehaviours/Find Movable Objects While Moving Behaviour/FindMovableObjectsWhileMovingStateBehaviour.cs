﻿using SJ.Tools;
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
            var movableObject = Owner.SearchMovableObjects();

            if (movableObject != null)
            {
                Blackboard.SetItem(Tribal.BlackboardKeys.MovableObject, movableObject);
                Trigger(Tribal.Trigger.Pull);
            }
        }
    }
}