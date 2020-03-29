using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SJ.Tools
{
    public class DoubleClickGraphic : SJMonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private float timeBetweenFirstAndSecondClick = 1;

        private SyncTimer timer = new SyncTimer();

        public Action OnTriggered;

        protected override void SJAwake()
        {
            timer.Loop = false;
            timer.onTick += _ => ResetTimer();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(timer.Active)
            {
                ResetTimer();
                OnTriggered?.Invoke();
            }
            else
            {
                PrepareTimer();
            }
        }

        private void PrepareTimer()
        {
            timer.Interval = timeBetweenFirstAndSecondClick;
            EnableUpdate = true;
            timer.Start();
        }

        protected override void SJUpdate()
        {
            timer.Update(Time.deltaTime);
        }

        private void ResetTimer()
        {
            EnableUpdate = false;
            timer.Stop();
        }
    }
}