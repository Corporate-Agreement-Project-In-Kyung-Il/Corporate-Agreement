using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataReceiverJiHun : MonoBehaviour
{
    public PlayerStat DependencyPlayerStat;
    public ScriptableObject HostStat;
    
    public int equipSelectionID;
    public int trainingSelectionID;
    
    private void Update()
    {
        
    }

    //public OptionChoice_EquipOption equip;

    //public void GetOptionValue()
    //{
    //    equip.GetValue(equipSelectionID);
    //}
    
    public void SetEquipSelectionID(int id)
    {
        equipSelectionID = GameManagerJiHun.Instance.optionButtons[id].selectID;
    }

    public void SetTrainingSelectionID(int id)
    {
        trainingSelectionID = GameManagerJiHun.Instance.optionButtons[id].selectID;
    }
}
