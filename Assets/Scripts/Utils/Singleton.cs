using UnityEngine;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T : Component 
    {
        private static T _instance = null;

        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    DontDestroyOnLoad(obj);
                    _instance = obj.AddComponent<T>();
                }
                return _instance;
            }
        }
    
        protected virtual void Init() {}

        private void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}