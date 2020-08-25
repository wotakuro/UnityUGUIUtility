using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        [SerializeField]
        private List<UIItem> bindInfos;

        private IUIBindable currentBindItem;

        public RectTransform rectTransform { get; private set; }

        private void Awake()
        {
            this.rectTransform = this.GetComponent<RectTransform>();
        }
        private void OnDisable()
        {
            if (currentBindItem != null)
            {
                this.currentBindItem.OnUnbind(this);
                this.currentBindItem = null;
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
            foreach (var bindInfo in bindInfos)
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
                var str = typeInfo.GetValue<string>(bindInfo.itemName, obj);
                bindInfo.itemObject.enabled = (str != null);
                if (str != null)
                {
                    Text text = (Text)bindInfo.itemObject;
                    text.text = str;
                }
            }
            else if (itemType == typeof(RawImage))
            {
                var texture = typeInfo.GetValue<Texture>(bindInfo.itemName, obj);
                bindInfo.itemObject.enabled = (texture != null);
                if (texture != null)
                {
                    RawImage rawImg = (RawImage)bindInfo.itemObject;
                    rawImg.texture = texture;
                }
            }
        }

        public List<UIItem> GetBindInfo()
        {
            if(bindInfos == null) { bindInfos = new List<UIItem>(); }
            return bindInfos;
        }
    }


}