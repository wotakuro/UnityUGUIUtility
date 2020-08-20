using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIUtility
{

    public class ScrollListItems : MonoBehaviour
    {
        private struct BufferObjectInfo
        {
            public UIItemBinder uiItem;
            public int index;

            public BufferObjectInfo(UIItemBinder item)
            {
                this.uiItem = item;
                this.index = -1;
            }
        }
        private class BufferObjectSort : IComparer<BufferObjectInfo>
        {
            public int Compare(BufferObjectInfo x, BufferObjectInfo y)
            {
                return x.index - y.index;
            }
        }

        public ScrollRect scrollRect;
        public UIItemBinder bindPrefab;
        private List<object> bindItems;

        private List<BufferObjectInfo> bufferObjects;

        private Rect itemRect;
        private Rect scrollItemRect;
        private BufferObjectSort compareObject = new BufferObjectSort();
        private int lastItemStartIdx = int.MinValue;

        private void Awake()
        {

            var obj = Instantiate(bindPrefab, scrollRect.content);
            this.scrollItemRect = scrollRect.GetComponent<RectTransform>().rect;
            this.itemRect = obj.GetComponent<RectTransform>().rect;
            this.CreateBufferObj();
            this.bufferObjects.Add(new BufferObjectInfo(obj));
            this.InitBufferObject();
            scrollRect.onValueChanged.AddListener(OnScrollChange);


            Debug.Log(scrollItemRect);
            this.scrollRect.content.ForceUpdateRectTransforms();

        }


        private IEnumerator Start()
        {
            List<int> vals = new List<int>();
            for (int i = 0; i < 50; ++i)
            {
                vals.Add(i);
            }


            this.Bind(vals);

            vals.Clear();
            for (int i = 0; i < 10; ++i)
            {
                vals.Add(i);
            }
            yield return new WaitForSeconds(2.0f);

            Debug.Log(scrollRect.GetComponent<RectTransform>().rect);
        }

        private void InitBufferObject()
        {
            for(int i = 0; i < bufferObjects.Count; ++i)
            {
                RectTransform rectTrans = bufferObjects[i].uiItem.GetComponent<RectTransform>();
                rectTrans.pivot = new Vector2(0.5f, 1f);
                rectTrans.anchorMin = new Vector2(0.5f, 1f);
                rectTrans.anchorMax = new Vector2(0.5f, 1f);
                rectTrans.ForceUpdateRectTransforms();
                bufferObjects[i].uiItem.gameObject.SetActive(false);
            }
        }

        private void CreateBufferObj()
        {
            int num = (int)(scrollItemRect.height / itemRect.height) + 1;
            this.bufferObjects = new List<BufferObjectInfo>(num+1);

            for ( int i = 0; i < num; ++i)
            {
                var obj = Instantiate(bindPrefab, scrollRect.content);
                bufferObjects.Add( new BufferObjectInfo(obj) );
            }
        }

        private void OnScrollChange(Vector2 val)
        {
            OnChangeScrollValue();
        }

        private void OnChangeScrollValue() { 
            float topPos = -scrollRect.content.localPosition.y;

            int itemIdx = (int)(-topPos / itemRect.height);

            if( itemIdx == this.lastItemStartIdx) { return; }
            int num = bufferObjects.Count;

            int minIdx = int.MaxValue;
            int maxIdx = int.MinValue;
            this.bufferObjects.Sort(compareObject);
            for (int i= 0; i < bufferObjects.Count; ++i)
            {
                var bufferObj = this.bufferObjects[i];
                if( bufferObj.index < 0) { continue; }
                if (minIdx > bufferObj.index)
                {
                    minIdx = bufferObj.index;
                }
                if (maxIdx < bufferObj.index)
                {
                    maxIdx = bufferObj.index;
                }
                if (bufferObj.index < itemIdx)
                {
                    SetBufferObjectAtPosition(i, -1);
                }
                if (bufferObj.index >= itemIdx + num)
                {
                    SetBufferObjectAtPosition(i, -1);
                }
            }
            this.bufferObjects.Sort(compareObject);
            int currentIdx = 0;
            // 足りない上側を補間
            for (int i = itemIdx; i < minIdx && i < itemIdx + num; ++i)
            {
                if (this.bufferObjects[currentIdx].index >= 0)
                {
                    return;
                }
                SetBufferObjectAtPosition(currentIdx, i);
                ++currentIdx;
            }
            for(int i = itemIdx + num -1; i >= maxIdx; --i)
            {
                if (currentIdx >= this.bufferObjects.Count ||
                    this.bufferObjects[currentIdx].index >= 0)
                {
                    return;
                }
                SetBufferObjectAtPosition(currentIdx, i);
                ++currentIdx;
            }
            this.lastItemStartIdx = itemIdx;
        }


        private void SetBufferObjectAtPosition(int bufIdx, int posIdx)
        {
            var bufferObj = this.bufferObjects[bufIdx];
            var position = new Vector3(0f, -itemRect.height * posIdx, 0); ;
            bufferObj.index = posIdx;
            
            bufferObj.uiItem.rectTransform.anchoredPosition = position;

            bufferObj.uiItem.gameObject.SetActive( posIdx >= 0);
            this.bufferObjects[bufIdx] = bufferObj;            
        }


        public void Bind<T>(IEnumerable<T> datas)
        {
            if( bindItems == null)
            {
                bindItems = new List<object>(1024);
            }
            bindItems.Clear();
            foreach( var data in datas)
            {
                bindItems.Add(data);
            }
            this.scrollRect.content.sizeDelta = 
                new Vector2(this.scrollRect.content.sizeDelta.x, this.itemRect.height * bindItems.Count);

            this.lastItemStartIdx = int.MinValue;
            this.OnChangeScrollValue();
        }
    }


}