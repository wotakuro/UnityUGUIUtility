using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

namespace UIUtility
{
    public class UIValueBindAttribute : System.Attribute
    {
        private string name;
        public UIValueBindAttribute(string n)
        {
            this.name = n;
        }
        public string GetName()
        {
            return this.name;
        }
    }


}