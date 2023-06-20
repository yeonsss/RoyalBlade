using System;
using Managers;
using UnityEngine;
using Utils;

namespace Controller.Creature.Projectile
{
    public class ProjectileController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            var damage = GameManager.instance.player.weapon.CalcDamage(out var critical);
            if (col.CompareTag("Enemy"))
            {
                col.GetComponent<MonsterController>().Attacked(damage, critical);
                SpawnManager.instance.ReturnInstance(gameObject, PoolType.Effect);
            }
        }
    }
}