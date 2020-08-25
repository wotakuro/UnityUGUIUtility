using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIUtility
{

    public class UIBindItem : MonoBehaviour
    {
        [System.Serializable]
        public struct UIItem
        {
            [SerializeField]
            public string itemName;
            [SerializeField]
            public Graphic itemObject;
        }
        [System.Serializable]
        public struct EventItem
        {
            [SerializeField]
            public string itemName;
            [SerializeField]
            public UIBindEventComponent eventTrigger;
        }


        [SerializeField]
        private List<UIItem> uiItemBindInfos;

        [SerializeField]
        private List<EventItem> eventItemBindInfos;

        private IUIBindable currentBindItem;
        public RectTransform rectTransform { get; private set; }

        private void Awake()
        {
            this.rectTransform = this.GetComponent<RectTransform>();
            this.InitClickEvent();
        }
        private void OnDisable()
        {
            if (currentBindItem != null)
            {
                this.currentBindItem.OnUnbind(this);
                this.currentBindItem = null;
            }
        }
        void InitClickEvent()
        {
            if(eventItemBindInfos == null) { return; }
            foreach(var eventItem in eventItemBindInfos)
            {
                var evtTrigger = eventItem.eventTrigger;
                evtTrigger.bindName = eventItem.itemName;
                evtTrigger.uIBindItem = this;
            }
        }
        internal void OnClickEvent(string str)
        {
            if (this.currentBindItem != null)
            {
                var type = this.currentBindItem.GetType();
                var typeInfo = UIBindTypeInfoManager.Instance.GetTypeInfo(type);
                typeInfo.InvokeOnClickAction(currentBindItem, str);
            }
        }

        public void Bind(IUIBindable data)
        {
            Bind(data, data.GetType() );
        }
        public void Bind(IUIBindable obj,System.Type type)
        {
            if(this.currentBindItem != null)
            {
                this.currentBindItem.OnUnbind(this);
            }

            var typeInfo = UIBindTypeInfoManager.Instance.GetTypeInfo(type);
            foreach (var bindInfo in uiItemBindInfos)
            {
                ApplyValue(obj, bindInfo, typeInfo);
            }
            this.currentBindItem = obj;
            this.currentBindItem.OnBind(this);
        }

        private void ApplyValue(IUIBindable obj,UIItem bindInfo,UIBindTypeInfo typeInfo)
        {
            var itemType = bindInfo.itemObject.GetType();
            if (itemType == typeof(Text))
            {
                var str = typeInfo.GetValue<string>(obj , bindInfo.itemName);
                bindInfo.itemObject.enabled = (str != null);
                if (str != null)
                {
                    Text text = (Text)bindInfo.itemObject;
                    text.text = str;
                }
            }
            else if (itemType == typeof(RawImage))
            {
                var texture = typeInfo.GetValue<Texture>(obj,bindInfo.itemName);
                bindInfo.itemObject.enabled = (texture != null);
                if (texture != null)
                {
                    RawImage rawImg = (RawImage)bindInfo.itemObject;
                    rawImg.texture = texture;
                }
            }
        }

        public List<UIItem> GetUIItemBindInfos()
        {
            if (uiItemBindInfos == null) { uiItemBindInfos = new List<UIItem>(); }
            return uiItemBindInfos;
        }
        public List<EventItem> GetEventItemBindInfos()
        {
            if (eventItemBindInfos == null) { eventItemBindInfos = new List<EventItem>(); }
            return eventItemBindInfos;
        }
    }


}