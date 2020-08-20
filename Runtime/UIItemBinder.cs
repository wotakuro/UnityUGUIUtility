using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIUtility
{

    public class UIItemBinder : MonoBehaviour
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
        public List<UIItem> bindInfos;


        public RectTransform rectTransform { get; private set; }

        private void Awake()
        {
            this.rectTransform = this.GetComponent<RectTransform>();
        }

        public void Bind<T>(T data)
        {
            Bind(data, typeof(T));
        }
        public void Bind(object obj,System.Type type)
        {
            var typeInfo = UIBindTypeInfoManager.Instance.GetTypeInfo(type);

            foreach (var bindInfo in bindInfos)
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
        }

        public List<UIItem> GetBindInfo()
        {
            if(bindInfos == null) { bindInfos = new List<UIItem>(); }
            return bindInfos;
        }
        public void SetBindInfo(List<UIItem> infos)
        {
            this.bindInfos = infos;
        }
    }


}