    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class CharacterChoosing : MonoBehaviour
    {
        public Text CharacterName;
        public Text Skill1Name;
        public Text Skill2Name;
        public Image Skill1Sprite;
        public Image Skill2Sprite;
        public Image CharacterSprite;

        [SerializeField] private PlayerList PlayerList;

        
        public void ShowInfo(CharacterChooseButton button)
        {
            CharacterSprite.sprite = button.CharacterSprite;
            Skill1Sprite.sprite = button.Skill1Sprite;
            Skill2Sprite.sprite = button.Skill2Sprite;
            CharacterName.text = button.characterName;
            Skill1Name.text = button.Skill1_info;
            Skill2Name.text = button.Skill2_info;
        }

        public void GetPlayerID(CharacterChooseButton button)
        {

            if (button.characterClass == character_class.전사)
            {
                PlayerList.CharacterIDs[0] = button.CharacterID;
            }
            else if (button.characterClass == character_class.궁수)
            {
                PlayerList.CharacterIDs[1] = button.CharacterID;
            }
            else if (button.characterClass == character_class.마법사)
            {
                PlayerList.CharacterIDs[2] = button.CharacterID;
            }


        }
    }
