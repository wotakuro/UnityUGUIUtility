using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using UIUtility;

namespace UIUtilityEditor
{
    [CustomEditor(typeof(UIBindItem))]
    public class UIItemBinderEditor:Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            UIBindItem obj = target as UIBindItem;
            if( obj == null) { return; }
            bool isDirty = false;
            isDirty |= OnGUIUIItems(obj);
            isDirty |= OnGUIEventItem(obj);

            if (isDirty)
            {
                EditorUtility.SetDirty(target);
            }
        }


        private bool OnGUIUIItems(UIBindItem obj)
        {
            bool isDirty = false;
            int deleteIndex = -1;
            var uiItemBindInfos = obj.GetUIItemBindInfos();
            EditorGUILayout.LabelField("表示設定");
            for (int i = 0; i < uiItemBindInfos.Count; ++i)
            {
                var info = uiItemBindInfos[i];
                EditorGUILayout.BeginHorizontal();
                string oldStr = info.itemName;
                Graphic oldGraphic = info.itemObject;

                info.itemName = EditorGUILayout.TextField(info.itemName);
                info.itemObject = EditorGUILayout.ObjectField(info.itemObject, typeof(Graphic), true) as Graphic;
                if (GUILayout.Button("X"))
                {
                    deleteIndex = i;
                    isDirty = true;
                }
                if (info.itemName != oldStr) { isDirty = true; }
                if (info.itemObject != oldGraphic) { isDirty = true; }
                uiItemBindInfos[i] = info;
                EditorGUILayout.EndHorizontal();
            }
            if (deleteIndex >= 0)
            {
                uiItemBindInfos.RemoveAt(deleteIndex);
            }
            if (GUILayout.Button("+"))
            {
                uiItemBindInfos.Add(new UIBindItem.UIItem());
                isDirty = true;
            }
            return isDirty;
        }


        private bool OnGUIEventItem(UIBindItem obj)
        {
            bool isDirty = false;
            int deleteIndex = -1;
            var uiItemBindInfos = obj.GetEventItemBindInfos();
            EditorGUILayout.LabelField("イベントアイテム設定");
            for (int i = 0; i < uiItemBindInfos.Count; ++i)
            {
                var info = uiItemBindInfos[i];
                EditorGUILayout.BeginHorizontal();
                string oldStr = info.itemName;
                var oldEvent = info.eventTrigger;

                info.itemName = EditorGUILayout.TextField(info.itemName);
                info.eventTrigger = EditorGUILayout.ObjectField(info.eventTrigger, typeof(UIBindEventComponent),true) as UIBindEventComponent;
                if (GUILayout.Button("X"))
                {
                    deleteIndex = i;
                    isDirty = true;
                }
                if (info.itemName != oldStr) { isDirty = true; }
                if (info.eventTrigger != oldEvent) { isDirty = true; }
                uiItemBindInfos[i] = info;
                EditorGUILayout.EndHorizontal();
            }
            if (deleteIndex >= 0)
            {
                uiItemBindInfos.RemoveAt(deleteIndex);
            }
            if (GUILayout.Button("+"))
            {
                uiItemBindInfos.Add(new UIBindItem.EventItem());
                isDirty = true;
            }
            return isDirty;
        }


    }


}