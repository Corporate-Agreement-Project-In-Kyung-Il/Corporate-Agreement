using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInformationGraphic : MonoBehaviour
{
    [Header("0번 characterClass, 1번 characterName, 2번 characterGrade \n 3번 training_level, 4번 equip_level \n 5번 health, 6번 attackDamage\n, 7번 criticalProbability\n, 9번 스킬1이름\n, 10번 스킬2이름, 11번 스킬1레벨\n, 12번 스킬1레벨\n")]
    [SerializeField] public TMP_Text[] text;
    [SerializeField] private Image[] image;
    
    public void UIPlayerInformationSetting(IBuffSelection playerStat)
    {
        string[] statArray = playerStat.buffplayerStat.ToStatArrayFromToString();

        for (int i = 0; i < statArray.Length; i++)
        {
            text[i].text = statArray[i];
        }

        text[statArray.Length].text = ((int)playerStat.buffplayerStat.attackDamage * 2).ToString();
        if (playerStat is Player player)
        {
            if (player.skills[0] is ISkillID skill1)
            {
                if (skill1 is ActiveSkillSO active1)
                    text[11].text = $"LV.{active1.Skill_current_LV}";
                else if (skill1 is BuffSO buff1)
                    text[11].text = $"LV.{buff1.Skill_current_LV}";
            }

            if (player.skills[1] is ISkillID skill2)
            {
                if (skill2 is ActiveSkillSO active2)
                    text[12].text = $"LV.{active2.Skill_current_LV}";
                else if (skill2 is BuffSO buff2)
                    text[12].text = $"LV.{buff2.Skill_current_LV}";
            }
        }
        
    }

    public void UISpirteSetting(ISpriteSelection playerSprite)
    {
        image[0].sprite = playerSprite.PlayerSprite;
        image[1].sprite = playerSprite.WeaponSprite;
        image[2].sprite = playerSprite.Skill1Icon;
        image[3].sprite = playerSprite.Skill2Icon;
        text[9].text = playerSprite.Skill1Name;
        text[10].text = playerSprite.Skill2Name;
    }

    

}
