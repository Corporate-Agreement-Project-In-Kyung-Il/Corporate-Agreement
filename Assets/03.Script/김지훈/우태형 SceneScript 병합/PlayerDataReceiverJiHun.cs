using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataReceiverJiHun : MonoBehaviour
{
    //Player에 InputGameManagerSkillID 메소드를 보면됨. 이때 0번째 = 전사, 1번째 = 궁수, 2번째 = 마법사
    [Header("Player에 InputGameManagerSkillID 메소드를 보면됨. 이때 0번째 = 전사, 1번째 = 궁수, 2번째 = 마법사")]
    
    //0번 전사, 1번 궁수, 2번 마법사
    public PlayerData[] Data;

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
                        Data[0].equip_level += equipOption.Equipment_LvUP;
                        Data[0].attackDamage += equipOption.Attack_LV_UP_Effect;
                        Data[0].health += equipOption.HP_LV_UP_Effect;
                        break;
                    case 1 :
                        Data[1].equip_level += equipOption.Equipment_LvUP;
                        Data[1].attackDamage += equipOption.Attack_LV_UP_Effect;
                        Data[1].health += equipOption.HP_LV_UP_Effect;
                        break;
                    case 2 :
                        Data[2].equip_level += equipOption.Equipment_LvUP;
                        Data[2].attackDamage += equipOption.Attack_LV_UP_Effect;
                        Data[2].health += equipOption.HP_LV_UP_Effect;
                        break;
                }
                TSVLoaderSample.OverwritePlayerData(Data[i]);
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
                DependencyPlayerStat[i].training_level += trainingOption.Training_LvUP;
                
                switch (i)
                {
                    case 0 :
                        Data[0].attackDamage += trainingOption.Critical_Damage_Increase;
                        Data[0].health += trainingOption.Critical_Damage_Increase;
                        Data[0].criticalProbability += trainingOption.Critical_Rate_Increase;
                        Data[0].training_level += trainingOption.Training_LvUP;
                        break;
                    case 1 :
                        Data[1].attackDamage += trainingOption.Critical_Damage_Increase;
                        Data[1].health += trainingOption.Critical_Damage_Increase;
                        Data[1].criticalProbability += trainingOption.Critical_Rate_Increase;
                        Data[1].training_level += trainingOption.Training_LvUP;
                        break;
                    case 2 :
                        Data[2].attackDamage += trainingOption.Critical_Damage_Increase;
                        Data[2].health += trainingOption.Critical_Damage_Increase;
                        Data[2].criticalProbability += trainingOption.Critical_Rate_Increase;
                        Data[2].training_level += trainingOption.Training_LvUP;
                        break;
                }
                TSVLoaderSample.OverwritePlayerData(Data[i]);
                break;
            }
            
        }
    }
    
}
