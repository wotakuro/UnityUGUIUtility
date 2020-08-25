using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIUtility
{
    public interface IUIBindable
    {
        void OnBind(UIBindItem bindUI);
        void OnUnbind(UIBindItem bindUI);
    }
}
