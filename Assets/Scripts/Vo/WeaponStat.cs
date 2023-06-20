using System.Collections.Generic;

namespace Vo
{
    public class Weapons
    {
        public List<WeaponStat> weaponStats { get; set; }
    }
    
    public class WeaponStat
    {
        public string name { get; set; }
        public float damage { get; set; }
        public int skillGaugeMount { get; set; }
        public float criticalDamageIncrease { get; set; }
        public float criticalChance { get; set; }
        public string spawnPath { get; set; }
    }
}