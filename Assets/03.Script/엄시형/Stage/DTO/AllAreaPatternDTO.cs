using System.Collections.Generic;

namespace _03.Script.엄시형.Stage.DTO
{
    /// <summary>
    /// 데이터 교환용 클래스
    /// 다른용도 사용금지
    /// </summary>
    public sealed class AllAreaPatternDTO
    {
        public List<AreaPatternDTO> AreaPatternList = new List<AreaPatternDTO>();
        
        public AllAreaPatternDTO(List<AreaPatternDTO> areaPatternList)
        {
            AreaPatternList = areaPatternList;
        }
    }
}