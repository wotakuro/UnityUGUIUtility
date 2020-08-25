using System.Collections.Generic;
using System.Reflection;

namespace UIUtility
{
    public class UIBindTypeInfo
    {
        private Dictionary<string, FieldInfo> fieldInfos;
        private Dictionary<string, PropertyInfo> propInfos;

        public UIBindTypeInfo(System.Type t)
        {
            var bindingFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var fields = t.GetFields(bindingFlag);
            var props = t.GetProperties(bindingFlag);

            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<UIValueBindAttribute>();
                if (attr != null)
                {
                    AddField(attr.GetName(), field);
                }
            }
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute<UIValueBindAttribute>();
                if (attr != null)
                {
                    AddProp(attr.GetName(), prop);
                }
            }
        }
        private void AddProp(string name, PropertyInfo prop)
        {
            if (propInfos == null)
            {
                propInfos = new Dictionary<string, PropertyInfo>();
            }
            propInfos.Add(name, prop);
        }
        private void AddField(string name, FieldInfo field)
        {
            if (fieldInfos == null)
            {
                fieldInfos = new Dictionary<string, FieldInfo>();
            }
            fieldInfos.Add(name, field);
        }

        public TVal GetValue<TVal>(string str, object obj) where TVal : class
        {
            if (fieldInfos != null)
            {
                FieldInfo field = null;
                if (fieldInfos.TryGetValue(str, out field))
                {
                    var val = field.GetValue(obj);
                    return val as TVal;
                }
            }
            if (propInfos != null)
            {
                PropertyInfo prop = null;
                if (propInfos.TryGetValue(str, out prop))
                {
                    var val = prop.GetValue(obj);
                    return val as TVal;
                }
            }
            return null;
        }
    }


}