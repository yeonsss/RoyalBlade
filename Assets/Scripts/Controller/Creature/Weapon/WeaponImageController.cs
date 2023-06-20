using System;
using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Controller.Creature.Weapon
{
    public class WeaponImageController : MonoBehaviour
    {
        [SerializeField] private WeaponController _weapon;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                _weapon.SlashEffect(col.transform.position);

                GameManager.instance.AddSkillGauge(_weapon.CalcGaugeMount());

                var damage = _weapon.CalcDamage(out var critical);
                col.GetComponent<MonsterController>().Attacked(damage, critical);
            }
        }
    }
}