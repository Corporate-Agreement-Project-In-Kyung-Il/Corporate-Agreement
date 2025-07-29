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
        [SerializeField] private ChoiceManager ChoiceManager;

        [SerializeField] private List<Button> PlayerChoosingButtons = new List<Button>();

        [SerializeField] private GameObject SameClass;
        [SerializeField] private GameObject ;
        public void ShowInfo(CharacterChooseButton button)
        {
            CharacterSprite.sprite = button.CharacterSprite;
            Skill1Sprite.sprite = button.Skill1Sprite;
            Skill2Sprite.sprite = button.Skill2Sprite;
            CharacterName.text = button.characterName;
            Skill1Name.text = button.Skill1_info;
            Skill2Name.text = button.Skill2_info;
            
            ChoiceManager.ButtonNumber = button.ButtonNumber;
        }

        public void GetPlayerID(CharacterChooseButton button)
        {

            if (button.characterClass == character_class.전사)
            {
                if (PlayerList.CharacterIDs[0] == 0)
                {
                    PlayerList.CharacterIDs[0] = button.CharacterID;
                }
                else if(PlayerList.CharacterIDs[0] == button.CharacterID)
                {
                    PlayerList.CharacterIDs[0] = 0;
                }
            }
            else if (button.characterClass == character_class.궁수)
            {
                if (PlayerList.CharacterIDs[1] == 0)
                {
                    PlayerList.CharacterIDs[1] = button.CharacterID;
                }
                else if(PlayerList.CharacterIDs[1] == button.CharacterID)
                {
                    PlayerList.CharacterIDs[1] = 0;
                }
            }
            else if (button.characterClass == character_class.마법사)
            {
                if (PlayerList.CharacterIDs[2] == 0)
                {
                    PlayerList.CharacterIDs[2] = button.CharacterID;
                }
                else if(PlayerList.CharacterIDs[2] == button.CharacterID)
                {
                    PlayerList.CharacterIDs[2] = 0;
                }
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

        private void SameClassPanelOnOff()
        {
            if (IfSameClass.activeSelf == false)
            {
                IfSameClass.SetActive(true);
            }
            else if (IfSameClass.activeSelf == true)
            {
                IfSameClass.SetActive(false);
            }
        }
        
        private void SameClassPanelOnOff()


        public void Pick_Character()
        {
            var characterChooseButton =
                PlayerChoosingButtons[ChoiceManager.ButtonNumber].GetComponent<CharacterChooseButton>();
            GetPlayerID(characterChooseButton);
        }

    }
