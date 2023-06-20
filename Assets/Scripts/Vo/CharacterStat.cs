using System.Collections.Generic;

namespace Vo
{
    public class Characters
    {
        public List<CharacterStat> characterStats { get; set; }
    }
    
    public class CharacterStat
    {
        public string name { get; set; }
        public float guardPower { get; set; }
        public float guardCoolDown { get; set; }
        public float jumpPower { get; set; }
        public string spawnPath { get; set; }
    }
}