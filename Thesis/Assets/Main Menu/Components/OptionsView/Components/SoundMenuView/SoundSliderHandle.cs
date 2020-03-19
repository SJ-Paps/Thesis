using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace SJ.Menu
{
    public class SoundSliderHandle : SJMonoBehaviour, IPointerUpHandler
    {
        public event Action OnDragEnd;

        public void OnPointerUp(PointerEventData eventData)
        {
            if(OnDragEnd != null)
            {
                OnDragEnd();
            }
        }
    }

}
