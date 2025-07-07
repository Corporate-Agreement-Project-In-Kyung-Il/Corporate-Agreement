using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OptionChoice", menuName = "LOH/OptionChoice_Training")]
public class OptionChoice_Training : ScriptableObject
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
    private string Training_ID;

    [SerializeField]
    private EMyEnum Selection_Level;
    
    [SerializeField]
    private string Description;
    

    public string selection_ID => Selection_ID;
    public string training_ID => Training_ID;
    public EMyEnum selection_Level => Selection_Level;
    public string description => Description;
}
