using System.Collections.Generic;

namespace Vo
{
    public class Items
    {
        public List<ItemStat> itemStats { get; set; }
    }
    
    public class ItemStat
    {
        public string name { get; set; }
    }
}