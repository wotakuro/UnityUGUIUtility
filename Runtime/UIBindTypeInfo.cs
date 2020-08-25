using System.Collections.Generic;
using System.Reflection;

namespace UIUtility
{
    internal class UIBindTypeInfo
    {
        private class VarInfo
        {
            public Dictionary<string, FieldInfo> fieldInfos;
            public Dictionary<string, PropertyInfo> propInfos;

            public void AddProp(string name, PropertyInfo prop)
            {
                if (propInfos == null)
                {
                    propInfos = new Dictionary<string, PropertyInfo>();
                }
                propInfos.Add(name, prop);
            }
            public void AddField(string name, FieldInfo field)
            {
                if (fieldInfos == null)
                {
                    fieldInfos = new Dictionary<string, FieldInfo>();
                }
                fieldInfos.Add(name, field);
            }
        }
        private VarInfo valueBindVarInfo = new VarInfo();

        private Dictionary<string, MethodInfo> onClickMethodInfos;

        public UIBindTypeInfo(System.Type t)
        {
            CollectVars< UIItemValueAttribute>(t, valueBindVarInfo);
            CollectMethodInfos<UIClickActionAttribute>(t, ref onClickMethodInfos);
        }

        private void CollectMethodInfos<T>(System.Type t,ref Dictionary<string, MethodInfo> methodInfos) where T : UIBindAttribute
        {
            var bindingFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var methods = t.GetMethods(bindingFlag);

            foreach(var method in methods)
            {
                var attr = method.GetCustomAttribute<T>();
                if(attr != null)
                {
                    if( methodInfos == null)
                    {
                        methodInfos = new Dictionary<string, MethodInfo>();
                    }
                    methodInfos.Add(attr.GetName(), method);
                }
            }
        }

        internal bool InvokeOnClickAction(IUIBindable obj,string str)
        {
            if(onClickMethodInfos == null) { return false; }
            MethodInfo method = null;
            if( onClickMethodInfos.TryGetValue(str,out method))
            {
                method.Invoke(obj,null);
                return true;
            }
            return false;
        }

        private void CollectVars<T>(System.Type t ,VarInfo info) where T: UIBindAttribute
        {
            var bindingFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var fields = t.GetFields(bindingFlag);
            var props = t.GetProperties(bindingFlag);

            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<T>();
                if (attr != null)
                {
                    valueBindVarInfo.AddField(attr.GetName(), field);
                }
            }
            foreach (var prop in props)
            {
                var attr = prop.GetCustomAttribute<T>();
                if (attr != null)
                {
                    valueBindVarInfo.AddProp(attr.GetName(), prop);
                }
            }

        }

        internal TVal GetValue<TVal>(IUIBindable obj,string str ) where TVal : class
        {
            if(valueBindVarInfo == null) { 
                return null; 
            }
            if (valueBindVarInfo.fieldInfos != null)
            {
                FieldInfo field = null;
                if (valueBindVarInfo.fieldInfos.TryGetValue(str, out field))
                {
                    var val = field.GetValue(obj);
                    return val as TVal;
                }
            }
            if (valueBindVarInfo.propInfos != null)
            {
                PropertyInfo prop = null;
                if (valueBindVarInfo.propInfos.TryGetValue(str, out prop))
                {
                    var val = prop.GetValue(obj);
                    return val as TVal;
                }
            }
            return null;
        }
    }


}