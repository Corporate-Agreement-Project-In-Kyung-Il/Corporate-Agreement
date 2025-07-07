using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelDescription
{
    public OptionChoice_EquipDic.EMyEnum level;
    public string description;
}
[CreateAssetMenu(fileName = "OptionChoice", menuName = "LOH/OptionChoice_EquipDic")]
public class OptionChoice_EquipDic : ScriptableObject
{
    public enum EMyEnum
    {
        노말,
        레어,
        에픽,
        유니크,
        레전드,
        신화
    }
    [SerializeField]
    private string Selection_ID;

    [SerializeField]
    private string Equipment_Type_ID;

    [SerializeField]
    private EMyEnum Selection_Level;
    
    [SerializeField]
    private List<LevelDescription> levelDescriptions;

    public string selection_ID => Selection_ID;
    public string equipment_Type_ID => Equipment_Type_ID;
    public EMyEnum selection_Level => Selection_Level;
    public string description
    {
        get
        {
            var item = levelDescriptions.Find(x => x.level == Selection_Level);
            return item != null ? item.description : string.Empty;
        }
    }
}
