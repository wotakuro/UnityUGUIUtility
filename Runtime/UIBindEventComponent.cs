using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIUtility
{
    public class UIBindEventComponent : MonoBehaviour,
        IPointerClickHandler,IPointerDownHandler,IPointerUpHandler,
        IPointerEnterHandler,IPointerExitHandler
    {
        [NonSerialized]
        public string bindName;

        [NonSerialized]
        public UIBindItem uIBindItem;

        public void OnPointerClick(PointerEventData eventData)
        {
            uIBindItem.OnClickEvent(bindName);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }
    }
}