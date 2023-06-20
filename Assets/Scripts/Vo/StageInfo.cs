using System.Collections.Generic;

namespace Vo
{
    public class States
    {
        public List<StageInfo> stageInfos { get; set; }
    }
    
    
    public class StageInfo
    {
        public List<List<string>> spawnList { get; set; }
    }
}