using System.Collections;
using Controller.Creature.Projectile;
using Managers;
using UnityEngine;
using Utils;

namespace Controller.Creature.Weapon
{
    public class DeathScytheController : WeaponController
    {
        private int _shootCount = 5;
        private float _radius = 0.5f;
        private float _height = 10f;

        public override void Skill()
        {
            var playerPos = GameManager.instance.player.transform.position;
            var leftPoint = playerPos + new Vector3(-_radius, 0, 0);
            var rightPoint = playerPos + new Vector3(_radius, 0, 0);

            StartCoroutine(ShootEffectCor("SkullEffect", leftPoint, rightPoint));
        }

        public override float CalcSkillDamage()
        {
            return DataManager.instance.weaponData[weaponName].damage * 3;
        }

        IEnumerator ShootEffectCor(string effectName, Vector3 leftPoint, Vector3 rightPoint)
        {
            for (int i = 0; i < _shootCount; i++)
            {
                var effect = SpawnManager.instance.SpawnEffect(effectName);
                effect.SetActive(true);
                if (i % 2 == 0)
                {
                    LeanTween.value(effect, Mathf.PI, 0, 0.5f)
                        .setOnUpdate((float value) =>
                        {
                            
                            var newPos = new Vector3(
                                leftPoint.x - Mathf.Cos(value) * _radius,
                                leftPoint.y - Mathf.Sin(value) * _radius,
                                leftPoint.z
                            );

                            effect.transform.position = newPos;

                        }).setEase(LeanTweenType.linear)
                        .setOnComplete(() =>
                        {
                            var pos = effect.transform.position;
                            LeanTween.moveY(effect, pos.y + _height, 0.5f)
                                .setOnComplete(() =>
                                {
                                    if (effect == null || effect.Equals(null))
                                    {
                                        return;
                                    }
                                    SpawnManager.instance.ReturnInstance(effect, PoolType.Effect);
                                });
                        });
                }
                else
                {
                    LeanTween.value(effect, 0, Mathf.PI, 0.5f)
                        .setOnUpdate((float value) =>
                        {
                            
                            var newPos = new Vector3(
                                rightPoint.x - Mathf.Cos(value) * _radius,
                                rightPoint.y - Mathf.Sin(value) * _radius,
                                rightPoint.z
                            );

                            effect.transform.position = newPos;

                        }).setEase(LeanTweenType.linear)
                        .setOnComplete(() =>
                        {
                            var pos = effect.transform.position;
                            LeanTween.moveY(effect, pos.y + _height, 0.5f)
                                .setOnComplete(() =>
                                {
                                    if (effect == null || effect.Equals(null))
                                    {
                                        return;
                                    }
                                    SpawnManager.instance.ReturnInstance(effect, PoolType.Effect);
                                });
                        });
                }

                yield return new WaitForSeconds(0.2f);
            }
        }
        
        public override void SlashEffect(Vector3 pos)
        {
            base.SlashEffect(pos);
        }
    }
}