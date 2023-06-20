using Managers;
using UnityEngine;

namespace Controller.Creature.Weapon
{
    public class KuiraController : WeaponController
    {
        public override void Skill()
        {
            GameManager.instance.AddLifePoint();
        }

        public override void SlashEffect(Vector3 pos)
        {
            GameManager.instance.StartEffectCor(
                "BloodEffect",
                "Blood",
                pos,
                0.55f
            );
        }
    }
}