using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

namespace UIUtility
{
    public class UIBindTypeInfoManager
    {
        public static UIBindTypeInfoManager Instance { get; set; } = new UIBindTypeInfoManager();

        private Dictionary<System.Type, UIBindTypeInfo> typeInfoDictionary;
        private UIBindTypeInfoManager()
        {
            typeInfoDictionary = new Dictionary<System.Type, UIBindTypeInfo>();
        }
        public UIBindTypeInfo GetTypeInfo(System.Type type)
        {
            UIBindTypeInfo val = null;
            if (!typeInfoDictionary.TryGetValue(type, out val))
            {
                val = new UIBindTypeInfo(type);
                typeInfoDictionary.Add(type, val);
            }

            return val;
        }
    }

}