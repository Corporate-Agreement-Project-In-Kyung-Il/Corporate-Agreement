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
        

        public void ShowInfo(CharacterChooseButton button)
        {
            CharacterSprite.sprite = button.CharacterSprite;
            Skill1Sprite.sprite = button.Skill1Sprite;
            Skill2Sprite.sprite = button.Skill2Sprite;
            CharacterName.text = button.characterName;
            Skill1Name.text = button.Skill1_info;
            Skill2Name.text = button.Skill2_info;
        }
    }
