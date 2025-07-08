using System;
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

        string filePath = Application.dataPath + "/05.DataTable/성장 데이터 종합.xlsx";
        if (!File.Exists(filePath))
        {
            Debug.LogError("엑셀 파일이 존재하지 않습니다: " + filePath);
            return;
        }

        using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var result = reader.AsDataSet();

        ImportCharacter(result.Tables[0]);   // 1번 시트 (Character)
        ImportSkillOption(result.Tables[4]);       // 5번 시트 (Skill)
        ImportEquipOption(result.Tables[6]);       // 7번 시트 (Equip)
        ImportTrainingOption(result.Tables[8]);    // 9번 시트 (Training)

        AssetDatabase.SaveAssets();
        Debug.Log("Excel 데이터 → ScriptableObject 변환 완료!");
    }

    private static void ImportCharacter(DataTable table)
    {
        var optionCharacter = ScriptableObject.CreateInstance<OptionChoice_Character>();
        for (int i = 2; i < table.Rows.Count; i++)
        {
            var strarr = GetTrimmedCells(table, i);

            var character = new Character
            {
                Character_ID = int.Parse(strarr[0]),
                Character_Class = strarr[1],
                Character_Name = strarr[2],
                Character_Grade = strarr[3],
                Attack = float.Parse(strarr[4]),
                Health = float.Parse(strarr[5]),
                Attack_Speed = float.Parse(strarr[6]),
                Critical_Probability = float.Parse(strarr[7]),
                Training_type = float.Parse(strarr[8]),
                equip_item = float.Parse(strarr[9]),
                skill_possed1 = float.Parse(strarr[10]),
                skill_possed2 = float.Parse(strarr[11])
            };

            var pair = new IDValuePair<Character> { Key_ID = character.Character_ID, val = character };
            optionCharacter.data.Add(pair);
        }

        AssetDatabase.CreateAsset(optionCharacter, $"Assets/00.Resources/OptionChoice/DataBase/CharacterOptionChoice.asset");
    }

    private static void ImportSkillOption(DataTable table)
    {
        var optionSkill = ScriptableObject.CreateInstance<OptionChoice_SkillOption>();
        for (int i = 2; i < table.Rows.Count; i++)
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

        AssetDatabase.CreateAsset(optionSkill, $"Assets/00.Resources/OptionChoice/DataBase/SkillOptionChoice.asset");
    }

    private static void ImportEquipOption(DataTable table)
    {
        var optionEquip = ScriptableObject.CreateInstance<OptionChoice_EquipOption>();
        for (int i = 2; i < table.Rows.Count; i++)
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

        AssetDatabase.CreateAsset(optionEquip, $"Assets/00.Resources/OptionChoice/DataBase/EquipOptionChoice.asset");
    }

    private static void ImportTrainingOption(DataTable table)
    {
        var optionTraining = ScriptableObject.CreateInstance<OptionChoice_TrainingOption>();
        for (int i = 2; i < table.Rows.Count; i++)
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

        AssetDatabase.CreateAsset(optionTraining, $"Assets/00.Resources/OptionChoice/DataBase/TrainingOptionChoice.asset");
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
