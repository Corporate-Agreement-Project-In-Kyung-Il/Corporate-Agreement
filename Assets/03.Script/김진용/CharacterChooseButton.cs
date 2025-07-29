using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterChooseButton : MonoBehaviour
{
     public int ButtonNumber;
    
     public Sprite CharacterSprite;
     public Sprite Skill1Sprite;
     public Sprite Skill2Sprite;
     public int CharacterID;
     public string Skill1_info;
     public string Skill2_info;
     public string characterName;
     public character_class characterClass;

     public GameObject Hand_img;
     public GameObject character_img;
     public GameObject character_name;
     private void Start()
     {
          character_name.GetComponent<Text>().text = characterName;
          CharacterSprite = character_img.GetComponent<Image>().sprite;
     }
}
