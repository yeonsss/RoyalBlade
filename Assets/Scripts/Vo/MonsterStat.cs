using System.Collections.Generic;

namespace Vo
{
    public class Monsters
    {
        public List<MonsterStat> monsterStats { get; set; }
    }
    
    public class MonsterStat
    {
        public string name { get; set; }
        public float hp { get; set; }
        public float dropPower { get; set; }
        public int score { get; set; }
    }
}