using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System;

namespace UIUtility
{
    public abstract class UIBindAttribute : System.Attribute
    {
        private string name;
        public UIBindAttribute(string n)
        {
            this.name = n;
        }
        public string GetName()
        {
            return this.name;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class UIItemValueAttribute : UIBindAttribute
    {
        public UIItemValueAttribute(string n) : base(n)
        {
        }
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class UIItemReferenceAttribute : UIBindAttribute
    {
        public UIItemReferenceAttribute(string n) : base(n)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class UIClickActionAttribute : UIBindAttribute
    {
        public UIClickActionAttribute(string n) : base(n)
        {

        }
    }

}