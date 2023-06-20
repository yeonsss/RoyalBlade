using System;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public static class Extension
    {
        public static void BindEvent(this GameObject go, Action action, Define.UIEvent type = Define.UIEvent.Click)
        {
            BaseUI.BindEvent(go, action, type);
        } 
        
        public static void BindEvent(this GameObject go, UnityAction<float> action, Define.UIEvent type = Define.UIEvent.ValueChange)
        {
            BaseUI.BindEvent(go, action, type);
        } 
    
        public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
        {
            return GetOrAddComponent<T>(go);
        }
    
        public static void SetParentActive(this GameObject go, bool active)
        {
            SetParentActive(go, active);
        } 
    }
}