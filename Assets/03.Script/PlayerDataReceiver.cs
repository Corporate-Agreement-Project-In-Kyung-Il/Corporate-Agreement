using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataReceiver : MonoBehaviour
{
    public int equipSelectionID;
    public int trainingSelectionID;
    
    public void SetEquipSelectionID(int id)
    {
        equipSelectionID = GameManager.Instance.optionButtons[id].selectID;
    }

    public void SetTrainingSelectionID(int id)
    {
        trainingSelectionID = GameManager.Instance.optionButtons[id].selectID;
    }
}
