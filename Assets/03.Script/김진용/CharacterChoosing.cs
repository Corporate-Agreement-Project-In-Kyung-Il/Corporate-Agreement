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

        [SerializeField] private List<Button> PlayerChoosingButtons = new List<Button>();
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
                ToggleSameClassButtons(button, character_class.전사);
            }
            else if (button.characterClass == character_class.궁수)
            {
                PlayerList.CharacterIDs[1] = button.CharacterID;
                ToggleSameClassButtons(button, character_class.궁수);
            }
            else if (button.characterClass == character_class.마법사)
            {
                PlayerList.CharacterIDs[2] = button.CharacterID;
                ToggleSameClassButtons(button, character_class.마법사);
            }
            HandpointOnOff(button);
        }

        private void HandpointOnOff(CharacterChooseButton button)
        {
            if (button.Hand_img.activeSelf == false)
            {
                button.Hand_img.SetActive(true);
            }
            else if (button.Hand_img.activeSelf == true)
            {
                button.Hand_img.SetActive(false);
            }
        }
        
        private void ToggleSameClassButtons(CharacterChooseButton selectedButton, character_class targetClass)
        {
            bool isAnyDisabled = false;

            // 같은 클래스 버튼 중 비활성화된 것이 있는지 확인
            foreach (Button btn in PlayerChoosingButtons)
            {
                CharacterChooseButton chooseBtn = btn.GetComponent<CharacterChooseButton>();
                if (chooseBtn != null && chooseBtn.characterClass == targetClass && btn.interactable == false)
                {
                    isAnyDisabled = true;
                    break;
                }
            }

            // 비활성화된 게 있으면 다시 활성화, 아니면 비활성화
            foreach (Button btn in PlayerChoosingButtons)
            {
                CharacterChooseButton chooseBtn = btn.GetComponent<CharacterChooseButton>();

                if (chooseBtn != null && chooseBtn != selectedButton && chooseBtn.characterClass == targetClass)
                {
                    btn.interactable = isAnyDisabled; // true로 다시 활성화하거나 false로 비활성화
                }
            }
        }
        
    }
