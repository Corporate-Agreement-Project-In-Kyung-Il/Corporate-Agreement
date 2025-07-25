using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "ScriptableObjects/Skill/Buff")]
public class BuffSO : ScriptableObject, ISkillID
{
    public int Skill_ID;
    public string Skill_Name;
    public SkillType Skill_Type;
    public int Skill_Minimum_LV;

    public int Skill_current_LV;
    public  character_class Skill_Class;
    public character_name Skill_Character;
    public bool Skill_Range;

    public int Skill_Maximum_LV;
    public float Skill_Cooldown;
    public float Skill_Duration;

    public string Skill_Buff_Type;
    public ScriptableObject Skill_Buff_Type_Object;
    public float Skill_Activation_Rate;

    public float Cooldown_Reduction;
    public float Duration_Increase;
    public float Activation_Rate_Increase;
    public GameObject SkillPrefab;
    
    public int SkillID { get; set; }
    public void SetSkillID()
    {
        SkillID = Skill_ID;
    }
    //
#if UNITY_EDITOR
    public void SetPrefab()
    {
        if (SkillPrefab == null)
        {
            // 프로젝트 내 모든 프리팹 검색
            string[] guids = AssetDatabase.FindAssets("t:prefab", new[] { "Assets/03.Script/Skill/SkillPrefab" });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                if (prefab != null)
                {
                    var components = prefab.GetComponents<MonoBehaviour>();

                    foreach (var comp in components)
                    {
                        if (comp == null)
                        {
                            continue;
                        }

                        // ISkillID를 구현한 컴포넌트를 찾고
                        if (comp is ISkillID skillScript)
                        {
                            Debug.Log(comp);
                            skillScript.SetSkillID();
                            // 스크립트의 SkillID가 SO의 Skill_ID와 같은지 비교
                            if (skillScript.SkillID == Skill_ID)
                            {
                                SkillPrefab = prefab;
                                EditorUtility.SetDirty(this);
                                return; // 찾았으면 종료
                            }
                        }
                    }
                }
            }
        }
    }
#endif
}
