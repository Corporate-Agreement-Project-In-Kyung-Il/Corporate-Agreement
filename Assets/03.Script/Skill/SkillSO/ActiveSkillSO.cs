using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

//
[CreateAssetMenu(fileName = "ActiveSkill", menuName = "ScriptableObjects/Skill/ActiveSkill")]
public class ActiveSkillSO : ScriptableObject, ISkillID
{
    public int SkillID { get; set; }

    public void SetSkillID()
    {
        SkillID = Skill_ID;
    }

    public int Skill_ID;
    public string Skill_Name;
    public SkillType Skill_Type;
    public int Skill_Minimum_LV;

    public int Skill_current_LV;
    public int Skill_Maximum_LV;
    public float Skill_Cooldown;
    public float Skill_Damage;
    public int Skill_Attack_Count;
    public bool Wide_Area;
    public float Skill_Range_width;
    public float Skill_Range_height;
    public float Cooldown_Reduction;
    public float Damage_Increase;

    public GameObject SkillPrefab;

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
