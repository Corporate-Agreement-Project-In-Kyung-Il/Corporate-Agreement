using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataReceiverJiHun : MonoBehaviour
{
    //Player에 InputGameManagerSkillID 메소드를 보면됨. 이때 0번째 = 전사, 1번째 = 궁수, 2번째 = 마법사
    [Header("Player에 InputGameManagerSkillID 메소드를 보면됨. 이때 0번째 = 전사, 1번째 = 궁수, 2번째 = 마법사")]
    
    [SerializeField] PlayerData warriorData;
    [SerializeField] PlayerData archerData;
    [SerializeField] PlayerData wizardData;
    public PlayerStat[] DependencyPlayerStat;
    
    
    //버프시킬 scriptAbleObject -> OptionChoice_EquipOption 혹은 OptionChoice_TrainingOption
    [Header("선택지가 EquipSelection일 떄, 선택된 Equip의 ID")]
    public int equipSelectionID;
    [Header("선택지가 TrainingSelection일 떄, 선택된 Training의 ID")]
    public int trainingSelectionID;
    
    
    
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
            
            if (DependencyPlayerStat[i].equip_item.Equals(equipOption.Equipment_Type_ID))
            {
                Debug.Log($"{DependencyPlayerStat[i].characterName} ::: {DependencyPlayerStat[i].equip_item} :::: {equipOption.Equipment_Type_ID}");
                DependencyPlayerStat[i].equip_level = equipOption.Equipment_LvUP;
                DependencyPlayerStat[i].attackDamage += equipOption.Attack_LV_UP_Effect;
                DependencyPlayerStat[i].health += equipOption.HP_LV_UP_Effect;

                switch (i)
                {
                    case 0 :
                        warriorData.equip_level = equipOption.Equipment_LvUP;
                        warriorData.attackDamage += equipOption.Attack_LV_UP_Effect;
                        warriorData.health += equipOption.HP_LV_UP_Effect;
                        break;
                    case 1 :
                        archerData.equip_level = equipOption.Equipment_LvUP;
                        archerData.attackDamage += equipOption.Attack_LV_UP_Effect;
                        archerData.health += equipOption.HP_LV_UP_Effect;
                        break;
                    case 2 :
                        wizardData.equip_level = equipOption.Equipment_LvUP;
                        wizardData.attackDamage += equipOption.Attack_LV_UP_Effect;
                        wizardData.health += equipOption.HP_LV_UP_Effect;
                        break;
                }
                break;
            }
        }
        StageEvent.OnTriggerEquipmentEvent();
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
          
            
            if (DependencyPlayerStat[i].training_type.Equals(trainingOption.Training_ID))
            {
                Debug.Log($"{DependencyPlayerStat[i].characterName} ::: {DependencyPlayerStat[i].training_type} :::: {trainingOption.Training_ID}");
                Debug.Log($"{trainingOption.Description} :::::: {trainingOption.Training_LvUP}");
                
                DependencyPlayerStat[i].attackDamage += trainingOption.Critical_Damage_Increase;
                DependencyPlayerStat[i].criticalProbability += trainingOption.Critical_Rate_Increase;
                DependencyPlayerStat[i].attackSpeed += trainingOption.Attack_Speed_Increase;
                
                switch (i)
                {
                    case 0 :
                        warriorData.attackDamage += trainingOption.Critical_Damage_Increase;
                        warriorData.health += trainingOption.Critical_Damage_Increase;
                        warriorData.criticalProbability += trainingOption.Critical_Rate_Increase;
                        break;
                    case 1 :
                        archerData.attackDamage += trainingOption.Critical_Damage_Increase;
                        archerData.health += trainingOption.Critical_Damage_Increase;
                        archerData.criticalProbability += trainingOption.Critical_Rate_Increase;
                        break;
                    case 2 :
                        wizardData.attackDamage += trainingOption.Critical_Damage_Increase;
                        wizardData.health += trainingOption.Critical_Damage_Increase;
                        wizardData.criticalProbability += trainingOption.Critical_Rate_Increase;
                        break;
                }
                break;
            }
        }
    }
    
}
