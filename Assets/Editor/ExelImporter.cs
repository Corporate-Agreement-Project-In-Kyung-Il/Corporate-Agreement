using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;
using ExcelDataReader;

public static class ExcelImporter
{
    [MenuItem("Tools/DataTable/Import Excel → ScriptableObjects")]
    public static void ImportDataTables()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(); // 리프레시 추가
        // 인코딩 등록
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        string playerStatFilePath = Application.dataPath + "/05.DataTable/성장 데이터 종합.xlsx";
        string monsterStatFilePath = Application.dataPath + "/05.DataTable/몬스터 스탯.xlsx";
        if (!File.Exists(playerStatFilePath))
        {
            Debug.LogError("엑셀 파일이 존재하지 않습니다: " + playerStatFilePath);
            
            return;
        }
        
        if (!File.Exists(monsterStatFilePath))
        {
            Debug.LogError("엑셀 파일이 존재하지 않습니다: " + monsterStatFilePath);
            
            return;
        }

        using var playerStream = File.Open(playerStatFilePath, FileMode.Open, FileAccess.Read);
        using var playerReader = ExcelReaderFactory.CreateReader(playerStream);
        var playerResult = playerReader.AsDataSet();

        ImportCharacter(playerResult.Tables[0]);         // 1번 시트 (Character)
        ImportBuff(playerResult.Tables[3]);              // 4번 시트 (BuffList)
        ImportActiveSkill(playerResult.Tables[1]);       // 2번 시트 (ActiveSkill)
        ImportBuffSkill(playerResult.Tables[2]);         // 3번 시트 (BuffSkill)         여기는 버프 리스트가 먼저 생성되어야 버프 스킬에 들어가서 List 먼저 생성하였습니다.
        ImportSkillOption(playerResult.Tables[4]);       // 5번 시트 (SkillOption)
        ImportEquip(playerResult.Tables[5]);             // 6번 시트 (Equip)
        ImportEquipOption(playerResult.Tables[6]);       // 7번 시트 (EquipOption)
        ImportTraining(playerResult.Tables[7]);          // 8번 시트 (Training)
        ImportTrainingOption(playerResult.Tables[8]);    // 9번 시트 (TrainingOption)
        AssetDatabase.SaveAssets();
        Debug.Log("Excel 데이터 → ScriptableObject 변환 완료!");
        
        
        using var monsterStream = File.Open(monsterStatFilePath, FileMode.Open, FileAccess.Read);
        using var monsterReader = ExcelReaderFactory.CreateReader(monsterStream);
        var monsterResult = monsterReader.AsDataSet();
        ImportMonsterStat(monsterResult.Tables[0]);
        AssetDatabase.SaveAssets();
    }

    private static void ImportMonsterStat(DataTable table)
    {
        var monsterStat = ScriptableObject.CreateInstance<MonsterStatExel>();
        for (int i = 3; i < table.Rows.Count; i++)
        {
            var strarr = GetTrimmedCells(table, i);

            var monster = new MonsterExel
            {
                Monster_HP = float.Parse(strarr[1]),
                Monster_Attack = float.Parse(strarr[2]),
                Monster_SpawnCount = int.Parse(strarr[3]),
                IsBossStage = int.Parse(strarr[4]) == 1 ? true : false,
                Boss_HP = float.Parse(strarr[5]),
                Boss_Attack = float.Parse(strarr[6])
            };

            var pair = new IDValuePair<MonsterExel> { Key_ID = int.Parse(strarr[0]), val = monster };
            monsterStat.data.Add(pair);
        }
        Debug.Log("Monster");
        AssetDatabase.CreateAsset(monsterStat, $"Assets/00.Resources/DataBase/MonsterStat.asset");
    }
    
    public static void ImportActiveSkill(DataTable table)
{
    for (int i = 1; i < table.Rows.Count; i++)
    {
        var row = table.Rows[i];
        if (row == null || row.ItemArray.Length < 13 || string.IsNullOrWhiteSpace(row[0]?.ToString()))
            continue;

        ActiveSkillSO skill = ScriptableObject.CreateInstance<ActiveSkillSO>();

        if (!int.TryParse(row[0]?.ToString(), out skill.Skill_ID)) continue;
        skill.Skill_Name = row[1]?.ToString();
        skill.Skill_Type = SkillType.active;

        int.TryParse(row[3]?.ToString(), out skill.Skill_Minimum_LV);
        int.TryParse(row[4]?.ToString(), out skill.Skill_Maximum_LV);
        float.TryParse(row[5]?.ToString(), out skill.Skill_Cooldown);
        float.TryParse(row[6]?.ToString(), out skill.Skill_Damage);
        int.TryParse(row[7]?.ToString(), out skill.Skill_Attack_Count);
        bool.TryParse(row[8]?.ToString(), out skill.Wide_Area);
        float.TryParse(row[9]?.ToString(), out skill.Skill_Range_width);
        float.TryParse(row[10]?.ToString(), out skill.Skill_Range_height);
        float.TryParse(row[11]?.ToString(), out skill.Cooldown_Reduction);
        float.TryParse(row[12]?.ToString(), out skill.Damage_Increase);

        string assetPath = $"Assets/00.Resources/DataBase/Skills/Active/{skill.Skill_Name}.asset";
        Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
        AssetDatabase.CreateAsset(skill, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        skill.SetPrefab();
        EditorApplication.delayCall += () =>
        {
            skill.SetPrefab();
            EditorUtility.SetDirty(skill);
            AssetDatabase.SaveAssets();
        };
    }
}

public static void ImportBuffSkill(DataTable table)
{
    // var allBuffTypes_List = AssetDatabase.LoadAllAssetsAtPath(($"Assets/00.Resources/DataBase/Skills/Active/"));
    //
    //
    // for (int i = 0; i < allBuffTypes_List.Length; i++)
    // {
    //     if (allBuffTypes_List[i] is BuffListSO buffListSO)
    //     {
    //         
    //     }
    // }
    List<BuffListSO> allBuffTypes = new List<BuffListSO>();
    string[] guids = AssetDatabase.FindAssets("t:BuffListSO", new[] { "Assets/00.Resources/DataBase/Skills/BuffList" });

    foreach (string guid in guids)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        var asset = AssetDatabase.LoadAssetAtPath<BuffListSO>(path);
        if (asset != null)
            allBuffTypes.Add(asset);
    }

    
    for (int i = 1; i < table.Rows.Count; i++)
    {
        var row = table.Rows[i];
        if (row == null || row.ItemArray.Length < 15 || string.IsNullOrWhiteSpace(row[0]?.ToString()))
            continue;

        BuffSO skill = ScriptableObject.CreateInstance<BuffSO>();

        if (!int.TryParse(row[0]?.ToString(), out skill.Skill_ID)) continue;
        skill.Skill_Name = row[1]?.ToString();
        skill.Skill_Type = SkillType.buff;

        int.TryParse(row[3]?.ToString(), out skill.Skill_Minimum_LV);

        if (!Enum.TryParse(row[4]?.ToString(), out character_class parsedClass))
        {
            Debug.LogWarning($"[BuffSO] character_class 파싱 실패: {row[4]} (i={i})");
            continue;
        }
        skill.Skill_Class = parsedClass;

        if (!Enum.TryParse(row[5]?.ToString(), out character_name parsedChar))
        {
            Debug.LogWarning($"[BuffSO] character_name 파싱 실패: {row[5]} (i={i})");
            continue;
        }
        skill.Skill_Character = parsedChar;

        bool.TryParse(row[6]?.ToString(), out skill.Skill_Range);
        int.TryParse(row[7]?.ToString(), out skill.Skill_Maximum_LV);
        float.TryParse(row[8]?.ToString(), out skill.Skill_Cooldown);
        float.TryParse(row[9]?.ToString(), out skill.Skill_Duration);
        skill.Skill_Buff_Type = row[10]?.ToString();
        float.TryParse(row[11]?.ToString(), out skill.Skill_Activation_Rate);
        float.TryParse(row[12]?.ToString(), out skill.Cooldown_Reduction);
        float.TryParse(row[13]?.ToString(), out skill.Duration_Increase);
        float.TryParse(row[14]?.ToString(), out skill.Activation_Rate_Increase);

        if (int.TryParse(skill.Skill_Buff_Type, out int buffTypeId))
        {
            foreach (var buffListSO in allBuffTypes)
            {
                if (buffListSO.Skill__Buff_Type_ID == buffTypeId)
                {
                    skill.Skill_Buff_Type_Object = buffListSO;
                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning($"[SkillSOGenerator] BuffType '{skill.Skill_Buff_Type}' → int 변환 실패 (i={i})");
        }

        string assetPath = $"Assets/00.Resources/DataBase/Skills/Buff/{skill.Skill_Name}.asset";
        Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
        AssetDatabase.CreateAsset(skill, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        skill.SetPrefab();
        EditorApplication.delayCall += () =>
        {
            skill.SetPrefab();
            EditorUtility.SetDirty(skill);
            AssetDatabase.SaveAssets();
        };
    }
}

public static void ImportBuff(DataTable table)
{
    for (int i = 1; i < table.Rows.Count; i++)
    {
        var row = table.Rows[i];
        if (row == null || row.ItemArray.Length < 3 || string.IsNullOrWhiteSpace(row[0]?.ToString()))
            continue;

        BuffListSO buff_list = ScriptableObject.CreateInstance<BuffListSO>();

        if (!int.TryParse(row[0]?.ToString(), out buff_list.Skill__Buff_Type_ID))
            continue;

        buff_list.SkillBuffTypeName = row[1]?.ToString();
        buff_list.Buff_Description = row[2]?.ToString();

        string assetPath = $"Assets/00.Resources/DataBase/Skills/BuffList/{buff_list.SkillBuffTypeName}.asset";
        Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
        AssetDatabase.CreateAsset(buff_list, assetPath);
    }

    AssetDatabase.SaveAssets();
    AssetDatabase.Refresh();
}

    
    private static void ImportCharacter(DataTable table)
    {
        var playerCharacter = ScriptableObject.CreateInstance<PlayerCharacter>();
        for (int i = 3; i < table.Rows.Count; i++)
        {
            var strarr = GetTrimmedCells(table, i);

            var character = new Character
            {
                Character_Class = strarr[1],
                Character_Name = strarr[2],
                Character_Grade = strarr[3],
                Attack = float.Parse(strarr[4]),
                Health = float.Parse(strarr[5]),
                Attack_Speed = float.Parse(strarr[6]),
                Critical_Probability = float.Parse(strarr[7]),
                Training_type = int.Parse(strarr[8]),
                equip_item = int.Parse(strarr[9]),
                skill_possed1 = int.Parse(strarr[10]),
                skill_possed2 = int.Parse(strarr[11])
            };

            var pair = new IDValuePair<Character> { Key_ID = int.Parse(strarr[0]), val = character };
            playerCharacter.data.Add(pair);
        }

        AssetDatabase.CreateAsset(playerCharacter, $"Assets/00.Resources/DataBase/Character.asset");
    }

    private static void ImportSkillOption(DataTable table)
    {
        var optionSkill = ScriptableObject.CreateInstance<OptionChoice_SkillOption>();
        for (int i = 3; i < table.Rows.Count; i++)
        {
            var strarr = GetTrimmedCells(table, i);

            var skill = new SkillOption
            {
                Skill_ID = int.Parse(strarr[1]),
                Selection_Level = (MyEnum)Enum.Parse(typeof(MyEnum), strarr[2]),
                Description = strarr[3],
                Cooldown_Reduction = float.Parse(strarr[4]),
                Duration_Increase = float.Parse(strarr[5]),
                Activation_Rate_Increase = float.Parse(strarr[6]),
                Damage_Increase = float.Parse(strarr[7]),
                Skill_LvUP = int.Parse(strarr[8])
            };

            var pair = new IDValuePair<SkillOption> { Key_ID = int.Parse(strarr[0]), val = skill };
            optionSkill.data.Add(pair);
        }

        AssetDatabase.CreateAsset(optionSkill, $"Assets/00.Resources/DataBase/OptionChoice/SkillOptionChoice.asset");
    }
    private static void ImportEquip(DataTable table)
    {
        var playerEquip = ScriptableObject.CreateInstance<PlayerEquip>();
        for (int i = 3; i < table.Rows.Count; i++)
        {
            var strarr = GetTrimmedCells(table, i);

            var equip = new Equip
            {
                Equipment_Type_Name = strarr[1],
                Equipment_Attack = float.Parse(strarr[2]),
                Equipment_HP = float.Parse(strarr[3]),
                Equipment_Minimum_LV = int.Parse(strarr[4]),
                Equipment_Maximum_LV = int.Parse(strarr[5]),
                Attack_LV_UP_Effect = float.Parse(strarr[6]),
                HP_LV_UP_Effect = float.Parse(strarr[7])
            };

            var pair = new IDValuePair<Equip> { Key_ID = int.Parse(strarr[0]), val = equip };
            playerEquip.data.Add(pair);
        }

        AssetDatabase.CreateAsset(playerEquip, $"Assets/00.Resources/DataBase/Equip.asset");
    }
    private static void ImportEquipOption(DataTable table)
    {
        var optionEquip = ScriptableObject.CreateInstance<OptionChoice_EquipOption>();
        for (int i = 3; i < table.Rows.Count; i++)
        {
            var strarr = GetTrimmedCells(table, i);

            var equip = new EquipOption
            {
                Equipment_Type_ID = int.Parse(strarr[1]),
                Selection_Level = (MyEnum)Enum.Parse(typeof(MyEnum), strarr[2]),
                Description = strarr[3],
                Attack_LV_UP_Effect = float.Parse(strarr[4]),
                HP_LV_UP_Effect = float.Parse(strarr[5]),
                Equipment_LvUP = int.Parse(strarr[6])
            };

            var pair = new IDValuePair<EquipOption> { Key_ID = int.Parse(strarr[0]), val = equip };
            optionEquip.data.Add(pair);
        }

        AssetDatabase.CreateAsset(optionEquip, $"Assets/00.Resources/DataBase/OptionChoice/EquipOptionChoice.asset");
    }
    private static void ImportTraining(DataTable table)
    {
        var playerTraining = ScriptableObject.CreateInstance<PlayerTraining>();
        for (int i = 3; i < table.Rows.Count; i++)
        {
            var strarr = GetTrimmedCells(table, i);

            var training = new Training
            {
                Training_Name = strarr[1],
                Critical_Rate = int.Parse(strarr[2]),
                Critical_Damage = float.Parse(strarr[3]),
                Attack_Speed = float.Parse(strarr[4]),
                Training_Minimum_LV = int.Parse(strarr[5]),
                Training_Maximum_LV = int.Parse(strarr[6]),
                Critical_Damage_Increase = float.Parse(strarr[7]),
                Critical_Rate_Increase = float.Parse(strarr[8]),
                Attack_Speed_Increase = float.Parse(strarr[9])
            };

            var pair = new IDValuePair<Training> { Key_ID = int.Parse(strarr[0]), val = training };
            playerTraining.data.Add(pair);
        }

        AssetDatabase.CreateAsset(playerTraining, $"Assets/00.Resources/DataBase/TrainingOptionChoice.asset");
    }
    private static void ImportTrainingOption(DataTable table)
    {
        var optionTraining = ScriptableObject.CreateInstance<OptionChoice_TrainingOption>();
        for (int i = 3; i < table.Rows.Count; i++)
        {
            var strarr = GetTrimmedCells(table, i);

            var training = new TrainingOption
            {
                Training_ID = int.Parse(strarr[1]),
                Selection_Level = (MyEnum)Enum.Parse(typeof(MyEnum), strarr[2]),
                Description = strarr[3],
                Critical_Damage_Increase = float.Parse(strarr[4]),
                Critical_Rate_Increase = float.Parse(strarr[5]),
                Attack_Speed_Increase = float.Parse(strarr[6]),
                Training_LvUP = int.Parse(strarr[7])
            };

            var pair = new IDValuePair<TrainingOption> { Key_ID = int.Parse(strarr[0]), val = training };
            optionTraining.data.Add(pair);
        }

        AssetDatabase.CreateAsset(optionTraining, $"Assets/00.Resources/DataBase/OptionChoice/TrainingOptionChoice.asset");
    }

    private static string[] GetTrimmedCells(DataTable table, int rowIndex)
    {
        var row = table.Rows[rowIndex];
        var cells = new string[table.Columns.Count];
        for (int j = 0; j < table.Columns.Count; j++)
        {
            cells[j] = row[j]?.ToString()?.Trim() ?? "";
        }
        return cells;
    }
}
