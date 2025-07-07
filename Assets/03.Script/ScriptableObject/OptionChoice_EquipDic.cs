using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OptionChoice", menuName = "LOH/OptionChoice_Equip")]
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
    private string Description;
    
    [SerializeField]
    private Dictionary<EMyEnum, string> GetLevelDescriptionDictionary()
    {
        var dict = new Dictionary<EMyEnum, string>
        {
            { Selection_Level, Description }
        };
        return dict;
    }
    

    public string selection_ID => Selection_ID;
    public string equipment_Type_ID => Equipment_Type_ID;
    public EMyEnum selection_Level => Selection_Level;
    public string description => Description;
    public Dictionary<EMyEnum, string> levelDescriptionDictionary => GetLevelDescriptionDictionary();
}
