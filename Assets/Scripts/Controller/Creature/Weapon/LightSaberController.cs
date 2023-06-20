using UnityEngine;

namespace Controller.Creature.Weapon
{
    public class LightSaberController : WeaponController
    {
        public override void Skill()
        {
            //TODO: 레이저를 발사.
        }

        public override void SlashEffect(Vector3 pos)
        {
            base.SlashEffect(pos);
        }
    }
}