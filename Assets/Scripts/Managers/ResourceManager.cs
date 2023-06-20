using UnityEngine;
using Utils;

namespace Managers
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        protected override void Init()
        {
            
        }

        public T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public GameObject Instantiate(string path, Vector3 pos = default, Quaternion rotate = default, Transform parent = null)
        {
            GameObject prefab = Load<GameObject>($"Prefabs/{path}");
            if (prefab == null)
            {
                return null;
            }

            var obj = Object.Instantiate(prefab, pos, rotate, parent);
            obj.name = prefab.name;
            return obj;
        }
        
        public Transform Instantiate(Transform origin, Vector3 pos = default, Quaternion rotate = default, Transform parent = null)
        {
            var obj = Object.Instantiate(origin, pos, rotate, parent);
            obj.name = origin.name;
            return obj;
        }
        
        public GameObject InstDontDestroy(string path, Vector3 pos, Quaternion rotate, Transform parent = null)
        {
            GameObject prefab = Load<GameObject>($"Prefabs/{path}");
            if (prefab == null)
            {
                return null;
            }
        
            var obj = Object.Instantiate(prefab, pos, rotate, parent);
            obj.name = prefab.name;
            DontDestroyOnLoad(obj);

            return obj;
        }

        public void SetDontDestroy(GameObject obj)
        {
            if (obj == null) return;
            DontDestroyOnLoad(obj);
        }

        public void Destroy(GameObject obj)
        {
            if (obj == null) return;
            UnityEngine.Object.Destroy(obj);
        }
    }
}