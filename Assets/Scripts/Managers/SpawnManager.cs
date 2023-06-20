using Controller.Creature;
using Controller.Creature.Player;
using TMPro;
using UnityEngine;
using Utils;

namespace Managers
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        private ObjectPool _spawnPool;
        
        protected override void Init()
        {
            _spawnPool = new ObjectPool();
        }
        
        public void ObjectPoolClear()
        {
            _spawnPool.InstanceAllDelete();
        }

        public GameObject SpawnMonster(string monsterName)
        {
            var obj = _spawnPool.GetInstance(monsterName, PoolType.Monster);
            var mc = obj.GetComponent<MonsterController>();
            mc.SetInfo(monsterName);
            mc.AnimParamClear();
            obj.SetActive(true);
            return obj;
        }

        public GameObject SpawnPlayer(string characterName)
        {
            var path = DataManager.instance.characterData[characterName].spawnPath;
            var obj = ResourceManager.instance.Instantiate(path, new Vector3(0, -2.4f, 0));
            obj.GetComponent<PlayerController>().characterName = characterName;
            return obj;
        }

        public GameObject SpawnRewardScoreMessage()
        {
            var obj = _spawnPool.GetInstance("RewardScore", PoolType.ScorePlus);
            return obj;
        }
        
        public GameObject SpawnDamageMessage()
        {
            var obj = _spawnPool.GetInstance("Damage", PoolType.Damage);
            return obj;
        }

        public GameObject SpawnEffect(string effectName)
        {
            var obj = _spawnPool.GetInstance(effectName, PoolType.Effect);
            return obj;
        }
        
        public void ReturnInstance(GameObject obj, PoolType type)
        {
            _spawnPool.ReturnInstance(obj, type);
        }
    }
}