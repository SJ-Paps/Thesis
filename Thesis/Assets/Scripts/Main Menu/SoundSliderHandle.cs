using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace SJ.UI
{
    public class SoundSliderHandle : SJMonoBehaviour, IPointerUpHandler
    {
        public event Action onDragEnd;

        public void OnPointerUp(PointerEventData eventData)
        {
            if(onDragEnd != null)
            {
                onDragEnd();
            }
        }
    }

}
