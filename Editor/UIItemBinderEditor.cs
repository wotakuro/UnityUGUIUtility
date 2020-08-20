using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using UnityEditor;
using UIUtility;
using UnityScript.Scripting.Pipeline;

namespace UIUtilityEditor
{
    [CustomEditor(typeof(UIItemBinder))]
    public class UIItemBinderEditor:Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            UIItemBinder obj = target as UIItemBinder;
            if( obj == null) { return; }
            var infos = obj.GetBindInfo();
            int deleteIndex = -1;
            bool isDirty = false;
            for(int i =0;i<infos.Count;++i)
            {
                var info = infos[i];
                EditorGUILayout.BeginHorizontal();
                string oldStr = info.itemName;
                Graphic oldGraphic = info.itemObject;

                info.itemName = EditorGUILayout.TextField(info.itemName);
                info.itemObject = EditorGUILayout.ObjectField(info.itemObject, typeof(Graphic),true) as Graphic;
                if( GUILayout.Button("X") ){
                    deleteIndex = i;
                    isDirty = true;
                }
                if (info.itemName != oldStr) { isDirty = true; }
                if (info.itemObject != oldGraphic) { isDirty = true; }
                infos[i] = info;
                EditorGUILayout.EndHorizontal();
            }
            serializedObject.ApplyModifiedProperties();


        }
    }


}