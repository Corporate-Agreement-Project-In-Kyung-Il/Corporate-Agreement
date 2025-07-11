using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataReceiverJiHun : MonoBehaviour
{
    //IBuffSelection 요소 
    public PlayerStat buffplayerStat { get; }
    
    public int equipSelectionID;
    public int trainingSelectionID;
    
    
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
