using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Utils;
using Vo;
using static Utils.Define;

namespace Managers
{
    public class DataManager : Singleton<DataManager>
    {
        public Dictionary<string, CharacterStat> characterData;
        public Dictionary<int, StageInfo> stageData;
        public Dictionary<string, ItemStat> itemData;
        public Dictionary<string, MonsterStat> monsterData;
        public Dictionary<string, WeaponStat> weaponData;

        private T DataInitialize<T>(string path)
        {
            TextAsset tAsset = Resources.Load<TextAsset>(path);
            T dataMapJson = JsonConvert.DeserializeObject<T>(tAsset.text);
            return dataMapJson;
        }
        
        protected override void Init()
        {
            characterData = new Dictionary<string, CharacterStat>();
            stageData = new Dictionary<int, StageInfo>();
            itemData = new Dictionary<string, ItemStat>();
            monsterData = new Dictionary<string, MonsterStat>();
            weaponData = new Dictionary<string, WeaponStat>();
            
            
            CharacterStatDataInit();
            StageInfoDataInit();
            ItemStatDataInit();
            MonsterStatDataInit();
            WeaponStatDataInit();
        }
        
        private void CharacterStatDataInit()
        {
            var data = DataInitialize<Characters>(CHARACTER_STAT_PATH);
            for (int i = 0; i < data.characterStats.Count; i++)
            {
                characterData.Add(data.characterStats[i].name, data.characterStats[i]);
            }
        }
        
        private void StageInfoDataInit()
        {
            var data = DataInitialize<States>(STAGE_INFO_PATH);
            for (int i = 0; i < data.stageInfos.Count; i++)
            {
                stageData.Add(i+1, data.stageInfos[i]);
            }
        }

        private void ItemStatDataInit()
        {
            var data = DataInitialize<Items>(ITEM_STAT_PATH);
            for (int i = 0; i < data.itemStats.Count; i++)
            {
                itemData.Add(data.itemStats[i].name, data.itemStats[i]);
            }
        }

        private void MonsterStatDataInit()
        {
            var data = DataInitialize<Monsters>(MONSTER_STAT_PATH);
            for (int i = 0; i < data.monsterStats.Count; i++)
            {
                monsterData.Add(data.monsterStats[i].name, data.monsterStats[i]);
            }
        }

        private void WeaponStatDataInit()
        {
            var data = DataInitialize<Weapons>(WEAPON_STAT_PATH);
            for (int i = 0; i < data.weaponStats.Count; i++)
            {
                weaponData.Add(data.weaponStats[i].name, data.weaponStats[i]);
            }
        }
    }
}