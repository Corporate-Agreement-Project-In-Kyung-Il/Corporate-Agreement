using System.Collections.Generic;
using UnityEngine;

public class TSVLoaderSample : MonoBehaviour
{
    public int currentStage;
    public class SampleData
    {
        public int? CurrentStage { get; set; }
        public int Character_ID { get; set; }
        public character_class Character_Class { get; set; }
        public character_name Character_Name { get; set; }
        public character_grade Character_Grade { get; set; }
        public float Attack { get; set; }
        public float Health {get; set;}
        public float Attack_Speed { get; set; }
        public float Critical_Probability { get; set;}
        public int Training_type { get; set; }
        public int Equip_Item { get; set; }
        public int Skill_Possed1 { get; set; }
        public int Skill_Possed2 { get; set; }
        public int Training_Level {get; set;}
        public int Equip_Level { get; set; }
        public int Skill1_Level { get; set; }
        public int Skill2_Level { get; set; }
    }
    
    public static List<SampleData> SampleDataList { get; set; }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    async void Awake()
    {
        //Unity의 StreamingAssets 경로, PersistentDataPath 경로 검색해서 스터디 할것.
        //아래 두 내용 주석 해제하면 어떤건지 나옴
        
        //Application.streamingAssetsPath
        //Application.persistentDataPath
        
        SampleDataList = await TSVLoader.LoadTableAsync<SampleData>("PlayerSaveFile", true);
        if (SampleDataList == null)
        {
            Debug.LogError("TSV 파일 로드 실패");
            return;
        }

        int? lastStage = null;

        foreach (var sampleData in SampleDataList)
        {
            if (sampleData.CurrentStage.HasValue)
                lastStage = sampleData.CurrentStage;
            else
                sampleData.CurrentStage = lastStage;

            Debug.Log(lastStage);
        }
        currentStage = (int)lastStage;
    }
}
