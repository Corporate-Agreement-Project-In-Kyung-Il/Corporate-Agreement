using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Data;
using ExcelDataReader;
using System.Text;

public class SkillSOGenerator
{
    /// <summary>
    /// 
    /// </summary>
    
    [MenuItem("Tools/Generate Skill ScriptableObjects")]
    public static void Generate()
    {
        string path = Application.dataPath + "/05.DataTable/성장 데이터 종합.xlsx";
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        using var stream = File.Open(path, FileMode.Open, FileAccess.Read);
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var result = reader.AsDataSet();

        // === BuffList 생성 ===
        DataTable buffList = result.Tables["Skill(버프 타입)"];
        for (int i = 1; i < buffList.Rows.Count; i++)
        {
            var row = buffList.Rows[i];
            if (row == null || row.ItemArray.Length < 3 || string.IsNullOrWhiteSpace(row[0]?.ToString()))
                continue;

            BuffListSO buff_list = ScriptableObject.CreateInstance<BuffListSO>();

            if (!int.TryParse(row[0]?.ToString(), out buff_list.Skill__Buff_Type_ID))
                continue;

            buff_list.SkillBuffTypeName = row[1]?.ToString();
            buff_list.Buff_Description = row[2]?.ToString();

            string assetPath = $"Assets/Resources/Skills/BuffList/{buff_list.SkillBuffTypeName}.asset";
            Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
            AssetDatabase.CreateAsset(buff_list, assetPath);
        }
        // === BuffListSO 먼저 로드 ===
        BuffListSO[] allBuffTypes = Resources.LoadAll<BuffListSO>("Skills/BuffList");
        GameObject[] allPrefabs = Resources.LoadAll<GameObject>("SkillPrefab");
       
        // === 액티브 스킬 ===
        DataTable activeTable = result.Tables["Skill(액티브)"];
        for (int i = 1; i < activeTable.Rows.Count; i++)
        {
            var row = activeTable.Rows[i];
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
            int.TryParse(row[9]?.ToString(), out skill.Skill_Range_width);
            int.TryParse(row[10]?.ToString(), out skill.Skill_Range_height);
            float.TryParse(row[11]?.ToString(), out skill.Cooldown_Reduction);
            float.TryParse(row[12]?.ToString(), out skill.Damage_Increase);

             // 여기가 핵심
            
            string assetPath = $"Assets/Resources/Skills/Active/{skill.Skill_Name}.asset";
            Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
            AssetDatabase.CreateAsset(skill, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            skill.SetPrefab();
            EditorApplication.delayCall += () =>
            {
                skill.SetPrefab();
                EditorUtility.SetDirty(skill); // 변경 사항 저장
                AssetDatabase.SaveAssets();
            };
            
        }

        // === 버프 스킬 ===
        DataTable buffTable = result.Tables["Skill(버프)"];
        for (int i = 1; i < buffTable.Rows.Count; i++)
        {
            var row = buffTable.Rows[i];
            if (row == null || row.ItemArray.Length < 15 || string.IsNullOrWhiteSpace(row[0]?.ToString()))
                continue;

            BuffSO skill = ScriptableObject.CreateInstance<BuffSO>();

            if (!int.TryParse(row[0]?.ToString(), out skill.Skill_ID)) continue;
            skill.Skill_Name = row[1]?.ToString();
            skill.Skill_Type = SkillType.buff;

            int.TryParse(row[3]?.ToString(), out skill.Skill_Minimum_LV);

            // 캐릭터 enum 파싱
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

            // === BuffListSO 연결 ===
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

            
            // 저장
            string assetPath = $"Assets/Resources/Skills/Buff/{skill.Skill_Name}.asset";
            Directory.CreateDirectory(Path.GetDirectoryName(assetPath));
            AssetDatabase.CreateAsset(skill, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            skill.SetPrefab();
            EditorApplication.delayCall += () =>
            {
                skill.SetPrefab();
                EditorUtility.SetDirty(skill); // 변경 사항 저장
                AssetDatabase.SaveAssets();
            };
        }
        
        
        
        Debug.Log("✅ 스킬 및 버프 타입 ScriptableObject 생성 완료!");
    }
}
