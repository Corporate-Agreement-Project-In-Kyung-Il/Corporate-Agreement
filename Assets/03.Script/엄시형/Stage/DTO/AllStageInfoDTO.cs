using System;
using System.Collections.Generic;

namespace _03.Script.엄시형.Stage.DTO
{
    [Serializable]
    public struct AllStageInfoDTO
    {
        public List<StageInfoDTO> StageInfoList;
        
        public AllStageInfoDTO(List<StageInfoDTO> stageInfoList)
        {
            StageInfoList = stageInfoList;
        }
    }
}