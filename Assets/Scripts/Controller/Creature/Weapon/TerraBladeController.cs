using System;
using System.Collections;
using Managers;
using UnityEngine;

namespace Controller.Creature.Weapon
{
    public class TerraBladeController : WeaponController
    {
        private bool _isSkillOn = false;

        public override void Skill()
        {
            // 몬스터의 떨어지는 속도랑 튕겼을 때 올라가는 속도를 늦춘다.
            StartCoroutine(MonsterMoveSlowCor());
        }

        private void Update()
        {
            if (_isSkillOn == false) return;
            GameManager.instance.MonsterMoveSlow();
        }

        IEnumerator MonsterMoveSlowCor()
        {
            _isSkillOn = true;
            yield return new WaitForSeconds(10);
            _isSkillOn = false;
            GameManager.instance.MonsterMoveNormal();
        }

        private void OnDestroy()
        {
            GameManager.instance.MonsterMoveNormal();
        }

        public override void SlashEffect(Vector3 pos)
        {
            base.SlashEffect(pos);
        }
    }
}