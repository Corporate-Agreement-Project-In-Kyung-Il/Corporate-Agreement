using System;
using System.Collections.Generic;

namespace _03.Script.엄시형.Stage.DTO
{
    [Serializable]
    public struct AllAreaPatternDTO
    {
        public List<AreaPatternDTO> AreaPatternList;
        
        public AllAreaPatternDTO(List<AreaPatternDTO> areaPatternList)
        {
            AreaPatternList = areaPatternList;
        }
        
    }
}