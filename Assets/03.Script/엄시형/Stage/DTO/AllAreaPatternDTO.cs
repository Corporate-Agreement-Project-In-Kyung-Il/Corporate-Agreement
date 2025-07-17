using System;
using System.Collections.Generic;

namespace _03.Script.엄시형.Stage.DTO
{
    /// <summary>
    /// 데이터 교환용 클래스
    /// 다른용도 사용금지
    /// </summary>
    [Serializable]
    public struct AllAreaPatternDTO
    {
        public List<AreaPatternDTO> AreaPatternList;
        
        public AllAreaPatternDTO(List<AreaPatternDTO> areaPatternList)
        {
            AreaPatternList = new List<AreaPatternDTO>();
            AreaPatternList = areaPatternList;
        }
    }
}