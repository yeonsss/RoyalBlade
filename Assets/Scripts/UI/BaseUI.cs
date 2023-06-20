using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Utils.Define;
using static Utils.Util;

namespace UI
{
    public abstract class BaseUI : MonoBehaviour
    {
        protected Dictionary<Type, UnityEngine.Object[]> _elements = new();

        public void Awake()
        {
            Init();
        }

        protected virtual void Init() { }

        protected void Bind<T>(Type type) where T : UnityEngine.Object
        {
            string[] names = Enum.GetNames(type);
            UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
            _elements.Add(typeof(T), objects);

            for (int i = 0; i < names.Length; i++)
            {
                if (typeof(T) == typeof(GameObject))
                    objects[i] = FindChild(gameObject, names[i], true);
                else
                    objects[i] = FindChild<T>(gameObject, names[i], true);

                if (objects[i] == null)
                    Debug.Log($"Failed to bind({names[i]})");
            }
        }

        protected T Get<T>(int idx) where T : UnityEngine.Object
        {
            UnityEngine.Object[] objects = null;
            if (_elements.TryGetValue(typeof(T), out objects) == false)
                return null;

            return objects[idx] as T;
        }
        
        // protected T GetForParent<T>(Transform obj, int idx) where T : UnityEngine.Object
        // {
        //     return obj.GetComponent<BaseUI>().Get<T>(idx);
        // }

        public static void BindEvent(GameObject go, Action action, UIEvent type = UIEvent.Click)
        {
            UIEventHandler evt = GetOrAddComponent<UIEventHandler>(go);

            switch (type)
            {
                case UIEvent.Click:
                    evt.OnClickHandler -= action;
                    evt.OnClickHandler += action;
                    break;
                case UIEvent.Pressed:
                    evt.OnPressedHandler -= action;
                    evt.OnPressedHandler += action;
                    break;
                case UIEvent.PointerDown:
                    evt.OnPointerDownHandler -= action;
                    evt.OnPointerDownHandler += action;
                    break;
                case UIEvent.PointerUp:
                    evt.OnPointerUpHandler -= action;
                    evt.OnPointerUpHandler += action;
                    break;
                
            }
        }
        
        public static void BindEvent(GameObject go, UnityAction<float> action, UIEvent type = UIEvent.ValueChange)
        {
            switch (type)
            {
                case UIEvent.ValueChange:
                    go.GetComponent<Slider>().onValueChanged.AddListener(action);
                    break;
            }
        }
    }

}