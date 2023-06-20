using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Managers;

namespace Utils
{
    public enum PoolType
    {
        Monster,
        Coin,
        ScorePlus,
        Damage,
        Effect,
    }
    
    public class ObjectStatus
    {
        public string name;
        public int instanceId;
        public DateTime createTime;
        public GameObject gameObject;
    }
    
    public class ObjectPool
    {
        private PoolType _type;
        public int size { get; }
        private Dictionary<PoolType, GameObject> _poolGameObj;
        private Dictionary<PoolType, List<ObjectStatus>> _poolList;

        public ObjectPool(PoolType type = 0, int poolSize = 10)
        {
            _type = type;
            size = poolSize;
            
            _poolGameObj = new Dictionary<PoolType, GameObject>();
            _poolList = new Dictionary<PoolType, List<ObjectStatus>>();

            foreach (var typeName in Enum.GetNames(typeof(PoolType)))
            {
                var obj = new GameObject
                {
                    name = $"{typeName}"
                };
                ResourceManager.instance.SetDontDestroy(obj);
                PoolType c = (PoolType)Enum.Parse(typeof(PoolType), typeName);
                _poolGameObj.Add(c, obj);
                _poolList.Add(c, new List<ObjectStatus>());
            }
        }

        public void InstanceAllDelete()
        {
            foreach (var poolInfo in _poolList)
            {
                var objList = poolInfo.Value;
                foreach (var os in objList)
                {
                    ResourceManager.instance.Destroy(os.gameObject);
                }
                objList.Clear();
            }            
        }

        private GameObject CreateInstance(string path, string objName, PoolType type)
        {
            var newInstance = ResourceManager.instance.Instantiate(path);
            newInstance.SetActive(false);

            if (_poolList[type].Count >= size)
            {
               DeleteInstance(type); 
            }

            if (_poolList[type].Count < size)
            {
                _poolList[type].Add(new ObjectStatus()
                {
                    name = objName,
                    instanceId = newInstance.GetInstanceID(),
                    createTime = DateTime.Now,
                    gameObject = newInstance
                });
                
                newInstance.transform.parent = _poolGameObj[type].transform;
            }

            return newInstance;
        }

        private void DeleteInstance(PoolType type)
        {
            var inActiveOs = _poolList[type].Find(
                os => os.gameObject.activeInHierarchy == false);
            
            if (inActiveOs == null) return;
            
            ResourceManager.instance.Destroy(inActiveOs.gameObject);
            _poolList[type].Remove(inActiveOs);
        }

        public GameObject GetInstance(string name, PoolType type)
        {
            // 인스턴스 아이디로 받아야 하는거 아닌가?
            
            // 이부분에서 기존에 있던 타워를 사용 안하는 듯.
            var activeOs = _poolList[type].Find(os => 
                os.name == name && os.gameObject.activeInHierarchy == false);

            if (activeOs == null)
            {
                switch (type)
                {
                    case PoolType.Monster :
                        return CreateInstance($"Creature/Monster/{name}", name, type);
                    case PoolType.Effect :
                        return CreateInstance($"Effect/{name}", name, type);
                    case PoolType.Coin :
                        return CreateInstance($"Creature/{name}", name, type);
                    case PoolType.ScorePlus :
                        return CreateInstance($"UI/{name}", name, type);
                    case PoolType.Damage :
                        return CreateInstance($"UI/{name}", name, type);
                    default:
                        return CreateInstance($"{name}", name, type);
                }
            }
            
            return activeOs.gameObject;
        }

        public void ReturnInstance(GameObject instance, PoolType type)
        {
            var activeOs = _poolList[type].Find(os => 
                os.instanceId == instance.GetInstanceID());
            
            if (activeOs != null) 
                activeOs.gameObject.SetActive(false);
                
            else
                ResourceManager.instance.Destroy(instance);
        }
    }
}