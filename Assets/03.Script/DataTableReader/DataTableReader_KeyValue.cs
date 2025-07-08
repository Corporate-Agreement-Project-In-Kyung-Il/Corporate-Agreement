using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using ExcelDataReader;
using UnityEditor;

public class DataTableReader_KeyValue : MonoBehaviour
{
    //기획자 분들 쓰실 수도 있어서 0번 시작이아니라 1번 시작입니다.
    [SerializeField] private int m_TableSheetNumber;
    void Start()
    {
        // 인코딩 등록 (한글 깨짐 방지)
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        // 엑셀 파일 경로
        string filePath = Application.dataPath + "/05.DataTable/성장 데이터 종합.xlsx";

        // 엑셀 열기
        using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var result = reader.AsDataSet();

        // N 번째 시트 가져오기
        DataTable table = result.Tables[m_TableSheetNumber - 1];

        string[] columnNames = new string[table.Columns.Count];
        for (int j = 0; j < table.Columns.Count; j++)
        {
            var cell = table.Rows[1][j]?.ToString();
            if (!string.IsNullOrEmpty(cell))
            {
                columnNames[j] = cell;
                Debug.Log(table.Rows.Count);
            }
        }

        string[] rowDatas = new string[table.Rows.Count - 2];
        // 첫 번째 행부터 전체 출력
        for (int i = 2; i < table.Rows.Count; i++)
        {
            string rowData = "";
            for (int j = 0; j < table.Columns.Count; j++)
            {
                var cell = table.Rows[i][j]?.ToString();
                if (!string.IsNullOrEmpty(cell))
                {
                    if (!string.IsNullOrEmpty(rowData))
                        rowData += " | ";
                    rowData += cell;
                }
            }
            rowDatas[i - 2] = rowData;
        }

        switch (m_TableSheetNumber)
        {
            case 1 :
#if UNITY_EDITOR
                var optionCharacter = ScriptableObject.CreateInstance<OptionChoice_Character>();

                for (int i = 0; i < rowDatas.Length; i++)
                {
                    var strarr = rowDatas[i].Split('|');
                    // 공백 제거
                    for (int k = 0; k < strarr.Length; k++)
                        strarr[k] = strarr[k].Trim();
            
                    int Character_ID = int.Parse(strarr[0]);
                    string Character_Class = strarr[1];
                    string Character_Name = strarr[2];
                    ECharacterGrade grade = (ECharacterGrade)Enum.Parse(typeof(ECharacterGrade), strarr[3]);
                    float Attack = float.Parse(strarr[4]);
                    float Health = float.Parse(strarr[5]);
                    float Attack_Speed = float.Parse(strarr[6]);
                    float Critical_Probability = float.Parse(strarr[7]);
                    float Training_type = float.Parse(strarr[8]);
                    float equip_item = float.Parse(strarr[9]);
                    float skill_possed1 = float.Parse(strarr[10]);
                    float skill_possed2 = float.Parse(strarr[11]);

                    var CharacterValue = new Character
                    {
                        Character_ID = Character_ID,
                        Character_Class = Character_Class,
                        Character_Name = Character_Name,
                        Character_Grade = grade.ToString(),
                        Attack = Attack,
                        Health = Health,
                        Attack_Speed = Attack_Speed,
                        Critical_Probability = Critical_Probability,
                        Training_type = Training_type,
                        equip_item = equip_item,
                        skill_possed1 = skill_possed1,
                        skill_possed2 = skill_possed2
                    };
            
                    var pair = new IDValuePair<Character>
                    {
                        Key_ID = Character_ID,
                        val = CharacterValue
                    };
            
                    optionCharacter.data.Add(pair);
                }
                AssetDatabase.CreateAsset(optionCharacter, $"Assets/00.Resources/OptionChoice/DataBase/CharacterOptionChoice.asset");
                AssetDatabase.SaveAssets();
#endif
                break;
            case 5 :
#if UNITY_EDITOR
                var optionSkill = ScriptableObject.CreateInstance<OptionChoice_SkillOption>();

                for (int i = 0; i < rowDatas.Length; i++)
                {
                    var strarr = rowDatas[i].Split('|');
                    // 공백 제거
                    for (int k = 0; k < strarr.Length; k++)
                        strarr[k] = strarr[k].Trim();
            
                    int key = int.Parse(strarr[0]);
                    int skill_ID = int.Parse(strarr[1]);
                    MyEnum grade = (MyEnum)Enum.Parse(typeof(MyEnum), strarr[2]);
                    string description = strarr[3];
                    float cooldownReduction = float.Parse(strarr[4]);
                    float durationIncrease = float.Parse(strarr[5]);
                    float activationRateIncrease = float.Parse(strarr[6]);
                    float damageIncrease = float.Parse(strarr[7]);
                    int skillLvUp = int.Parse(strarr[8]);
            
                    var skillValue = new SkillOption
                    {
                        Skill_ID = skill_ID,
                        Selection_Level = grade,
                        Description = description,
                        Cooldown_Reduction = cooldownReduction,
                        Duration_Increase = durationIncrease,
                        Activation_Rate_Increase = activationRateIncrease,
                        Damage_Increase = damageIncrease,
                        Skill_LvUP = skillLvUp
                    };
            
                    var pair = new IDValuePair<SkillOption>
                    {
                        Key_ID = key,
                        val = skillValue
                    };
            
                    optionSkill.data.Add(pair);
                }
                AssetDatabase.CreateAsset(optionSkill, $"Assets/00.Resources/OptionChoice/DataBase/SkillOptionChoice.asset");
                AssetDatabase.SaveAssets();
#endif
                break;
            case 7 : 
#if UNITY_EDITOR
                var optionEquip = ScriptableObject.CreateInstance<OptionChoice_EquipOption>();

                for (int i = 0; i < rowDatas.Length; i++)
                {
                    var strarr = rowDatas[i].Split('|');
                    // 공백 제거
                    for (int k = 0; k < strarr.Length; k++)
                        strarr[k] = strarr[k].Trim();
            
                    int key = int.Parse(strarr[0]);
                    int equipmentTypeId = int.Parse(strarr[1]);
                    MyEnum grade = (MyEnum)Enum.Parse(typeof(MyEnum), strarr[2]);
                    string description = strarr[3];
                    float attackLvUpEffect = float.Parse(strarr[4]);
                    float hpLvUpEffect = float.Parse(strarr[5]);
                    int equipmentLvUp = int.Parse(strarr[6]);
            
                    var value = new EquipOption
                    {
                        Equipment_Type_ID = equipmentTypeId,
                        Selection_Level = grade,
                        Description = description,
                        Attack_LV_UP_Effect = attackLvUpEffect,
                        HP_LV_UP_Effect = hpLvUpEffect,
                        Equipment_LvUP = equipmentLvUp
                    };
            
                    var pair = new IDValuePair<EquipOption>
                    {
                        Key_ID = key,
                        val = value
                    };
            
                    optionEquip.data.Add(pair);
                }
                AssetDatabase.CreateAsset(optionEquip, $"Assets/00.Resources/OptionChoice/DataBase/EquipOptionChoice.asset");
                AssetDatabase.SaveAssets();
#endif
                break;
            case 9 :
#if UNITY_EDITOR
                var optionTraining = ScriptableObject.CreateInstance<OptionChoice_TrainingOption>();

                for (int i = 0; i < rowDatas.Length; i++)
                {
                    var strarr = rowDatas[i].Split('|');
                    // 공백 제거
                    for (int k = 0; k < strarr.Length; k++)
                        strarr[k] = strarr[k].Trim();
            
                    int key = int.Parse(strarr[0]);
                    int trainingId = int.Parse(strarr[1]);
                    MyEnum grade = (MyEnum)Enum.Parse(typeof(MyEnum), strarr[2]);
                    string description = strarr[3];
                    float criticalDamageIncrease = float.Parse(strarr[4]);
                    float criticalRateIncrease = float.Parse(strarr[5]);
                    float attackSpeedIncrease = float.Parse(strarr[6]);
                    int trainingLvUp = int.Parse(strarr[7]);
            
                    var value = new TrainingOption
                    {   
                        Training_ID = trainingId,
                        Selection_Level = grade,
                        Description = description,
                        Critical_Damage_Increase = criticalDamageIncrease,
                        Critical_Rate_Increase = criticalRateIncrease,
                        Attack_Speed_Increase = attackSpeedIncrease,
                        Training_LvUP = trainingLvUp
                    };
            
                    var pair = new IDValuePair<TrainingOption>
                    {
                        Key_ID = key,
                        val = value
                    };
            
                    optionTraining.data.Add(pair);
                }
                AssetDatabase.CreateAsset(optionTraining, $"Assets/00.Resources/OptionChoice/DataBase/TrainingOptionChoice.asset");
                AssetDatabase.SaveAssets();
#endif
                break;
        }
    }
}
