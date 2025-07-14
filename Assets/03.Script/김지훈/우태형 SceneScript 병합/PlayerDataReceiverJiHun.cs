using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataReceiverJiHun : MonoBehaviour
{
    //Player에 InputGameManagerSkillID 메소드를 보면됨. 이때 0번째 = 전사, 1번째 = 궁수, 2번째 = 마법사
    public PlayerStat[] DependencyPlayerStat;
    
    //버프시킬 scriptAbleObject -> OptionChoice_EquipOption 혹은 OptionChoice_TrainingOption
    public int equipSelectionID;
    public int trainingSelectionID;
    

    //public OptionChoice_EquipOption equip;

    //public void GetOptionValue()
    //{
    //    equip.GetValue(equipSelectionID);
    //}
    
    public void SetEquipSelectionID(int id)
    {
        equipSelectionID = GameManager.Instance.optionButtons[id].selectID;

        var selectedDataEquip = GameManager.Instance.optionButtons[id].selectedData;
        var equipOption = selectedDataEquip as EquipOption;
        
        
        Debug.Log($"{equipOption.Equipment_LvUP}");
        Debug.Log($"{equipOption.Equipment_Type_ID}");
        Debug.Log($"{equipOption.Description}");
        Debug.Log($"{equipOption.Selection_Level}");
        Debug.Log($"{equipOption.Attack_LV_UP_Effect}");
        Debug.Log($"{equipOption.HP_LV_UP_Effect}");
        
        for (int i = 0; i < DependencyPlayerStat.Length; i++)
        {
            Debug.Log($"{DependencyPlayerStat[i].equip_item} :::: {equipOption.Equipment_Type_ID}");
            
            if (DependencyPlayerStat[i].equip_item.Equals(equipOption.Equipment_Type_ID))
            {
                Debug.Log($"{equipOption.Description} :::::: {equipOption.Equipment_LvUP}");
                DependencyPlayerStat[i].attackDamage += equipOption.Attack_LV_UP_Effect;
                DependencyPlayerStat[i].health += equipOption.HP_LV_UP_Effect;
                break;
            }
        }
        
        // HostStat = GameManagerJiHun.Instance.
    }
    
    public void SetTrainingSelectionID(int id)
    {
        trainingSelectionID = GameManager.Instance.optionButtons[id].selectID;
        
        
        var selected = GameManager.Instance.optionButtons[id].selectedData;
        var trainingOption = selected as TrainingOption;
            
        Debug.Log($"{trainingOption.Training_LvUP}");
        Debug.Log($"{trainingOption.Selection_Level}");
        Debug.Log($"{trainingOption.Description}");
        Debug.Log($"{trainingOption.Attack_Speed_Increase}");
        Debug.Log($"{trainingOption.Critical_Damage_Increase}");
        Debug.Log($"{trainingOption.Critical_Rate_Increase}");
        Debug.Log($"{trainingOption.Training_ID}");
        
        for (int i = 0; i < DependencyPlayerStat.Length; i++)
        {
            Debug.Log($"{DependencyPlayerStat[i].training_type} :::: {trainingOption.Training_ID}");
            
            if (DependencyPlayerStat[i].training_type.Equals(trainingOption.Training_ID))
            {
                Debug.Log($"{trainingOption.Description} :::::: {trainingOption.Training_LvUP}");
                
                DependencyPlayerStat[i].attackDamage += trainingOption.Critical_Damage_Increase;
                DependencyPlayerStat[i].criticalProbability += trainingOption.Critical_Rate_Increase;
                DependencyPlayerStat[i].attackSpeed += trainingOption.Attack_Speed_Increase;
                break;
            }
        }
    }
    
}
