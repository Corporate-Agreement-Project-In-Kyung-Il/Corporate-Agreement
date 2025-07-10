using System.Collections.Generic;

namespace _03.Script.엄시형.Stage.DTO
{
    public class AllAreaPatternDTO
    {
        public List<AreaPatternDTO> AreaPatternList = new List<AreaPatternDTO>();

        public AllAreaPatternDTO(List<AreaPatternDTO> areaPatternList)
        {
            AreaPatternList = areaPatternList;
        }
    }
}