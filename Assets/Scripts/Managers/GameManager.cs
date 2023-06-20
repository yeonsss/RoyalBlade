using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Controller.Creature;
using Controller.Creature.Player;
using UI.Popups;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using static Utils.Define;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public PlayerController player;
        public int score;
        public int combo;
        public int remainMonsterCount;
        public List<GameObject> spawnMonsterList;

        private float _spawnYPos = 20f;
        
        public bool isGameStart { get; set; }
        public int lifePoint { get; set; }
        public int maxLifePoint = 10;
        public int stageNum { get; set; }
        public int roundNum { get; set; }

        public float skillGauge { get; set; }

        public float specialGauge { get; set; }
        
        public bool isGameClear { get; set; }

        protected override void Init()
        {
            lifePoint = baseLifePoint;
            isGameStart = true;
            stageNum = 1;
            roundNum = 0;
            score = 0;
            combo = 0;
            skillGauge = 0;
            specialGauge = 0;
            remainMonsterCount = 0;
            spawnMonsterList = new List<GameObject>();
        }

        private void Update()
        {
            if (lifePoint == 0) return;
            if (isGameClear == true) return;

            GameClearCheck();
            if (isGameClear == false && remainMonsterCount == 0)
            {
                SpawnMonsterBlock();
            }
        }

        private void GameClearCheck()
        {
            var lastStage = DataManager.instance.stageData.Count;
            if (stageNum > lastStage)
            {
                isGameClear = true;
            }
        }

        public void AddLifePoint()
        {
            lifePoint = Mathf.Clamp(lifePoint + 1, 0, maxLifePoint);
        }

        public void SubLifePoint()
        {
            lifePoint = Mathf.Clamp(lifePoint - 1, 0, maxLifePoint);
        }

        public void DisplayDamage(float damage, bool critical)
        {
            var obj = SpawnManager.instance.SpawnDamageMessage();
            obj.transform.position = player.transform.position + new Vector3(0, 0.5f, 0);
            var dm = obj.GetComponent<Damage>();
            dm.SetInfo(damage, critical);
            obj.SetActive(true);
            dm.StartMoveUp();
        }

        public void AddScore(int rewardScore)
        {
            score += rewardScore;
            var obj = SpawnManager.instance.SpawnRewardScoreMessage();
            obj.transform.position = player.transform.position + new Vector3(1f, -0.5f, 0); 
            var sp = obj.GetComponent<ScorePlus>();
            sp.SetMessage(rewardScore.ToString());
            obj.SetActive(true);
            sp.StartMoveUp();
        }

        public void AddSkillGauge(float value)
        {
            skillGauge = Mathf.Clamp(skillGauge + value, 0, 100);
        }
        
        public void AddSpecialGauge(float value)
        {
            specialGauge = Mathf.Clamp(specialGauge + value, 0, 100);
        }

        public void InitTotalVelocity()
        {
            foreach (var monster in spawnMonsterList)
            {
                monster.GetComponent<MonsterController>().InitVelocity();
            }
        }
        
        public void SetAllMonsterVelocity(float velY)
        {
            foreach (var monster in spawnMonsterList)
            {
                monster.GetComponent<MonsterController>().PushForce(velY);
            }
        }

        public void StartEffectCor(string objectName, string animName, Vector3 pos, float time)
        {
            var cor = GameManager.instance.EffectCor(
                objectName,
                animName,
                pos,
                time
            );
            StartCoroutine(cor);
        }

        public IEnumerator EffectCor(string objectName, string animName, Vector3 pos, float time)
        {
            var effect = SpawnManager.instance.SpawnEffect(objectName);
            effect.SetActive(true);
            effect.transform.position = pos;
            effect.GetComponent<Animator>().Play(animName);
            yield return new WaitForSeconds(time);
            SpawnManager.instance.ReturnInstance(effect, PoolType.Effect);
        }

        public void MonsterMoveSlow()
        {
            foreach (var monster in spawnMonsterList)
            {
                if (monster == null || monster.Equals(null))
                    continue;
                
                var mc = monster.GetComponent<MonsterController>();
                if (mc.isDied == false)
                {
                    mc.isSlowed = true;
                }
            }
        }
        

        public void MonsterMoveNormal()
        {
            foreach (var monster in spawnMonsterList)
            {
                if (monster == null || monster.Equals(null))
                    continue;
                
                var mc = monster.GetComponent<MonsterController>();
                if (mc.isDied == false)
                {
                    mc.isSlowed = false;
                }
            }
        }

        public void SpawnMonsterBlock()
        {
            spawnMonsterList.Clear();
            
            var data = DataManager.instance.stageData[stageNum];

            if (roundNum + 1 > data.spawnList.Count)
            {
                stageNum += 1;
                roundNum = 0;
                player.ChangeWeapon();
                return;
            }
            
            remainMonsterCount = data.spawnList[roundNum].Count;
            var currentSpawnPos = _spawnYPos;

            foreach (var mName in data.spawnList[roundNum])
            {
                var monster = SpawnManager.instance.SpawnMonster(mName);
                monster.GetComponent<MonsterController>().SetInfo(mName);
                monster.transform.position = new Vector3(0, currentSpawnPos, 0);
                spawnMonsterList.Add(monster);
                var bosSizeY = monster.GetComponent<BoxCollider2D>().size.y;
                var offset = monster.transform.localScale.y * bosSizeY / 2;
                currentSpawnPos += offset;
            }

            roundNum++;
        }

        public void GameReStart()
        {
            foreach (var monster in spawnMonsterList)
            {
                if (monster == null || monster.Equals(null)) continue;
                SpawnManager.instance.ReturnInstance(monster, PoolType.Monster);
            }
            
            spawnMonsterList.Clear();
            
            lifePoint = baseLifePoint;
            isGameStart = true;
            stageNum = 1;
            roundNum = 0;
            score = 0;
            combo = 0;
            skillGauge = 0;
            specialGauge = 0;
            remainMonsterCount = 0;
            isGameClear = false;
            player.WeaponInit();
        }

        public void GameStop()
        {
            isGameStart = false;
            Time.timeScale = 0f;
        }

        public void GameResume()
        {
            isGameStart = true;
            Time.timeScale = 1f;
        }
    }
}