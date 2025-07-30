using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInformationGraphic : MonoBehaviour
{
    [Header("0번 characterClass, 1번 characterName, 2번 characterGrade \n 3번 training_level, 4번 equip_level \n 5번 health, 6번 attackDamage\n, 7번 criticalProbability")]
    [SerializeField] private TMP_Text[] text;
    [SerializeField] private Image[] image;
    
    public void UIPlayerInformationSetting(IBuffSelection playerStat)
    {
        string[] statArray = playerStat.buffplayerStat.ToStatArrayFromToString();

        for (int i = 0; i < statArray.Length; i++)
        {
            text[i].text = statArray[i];
        }

        text[statArray.Length].text = (playerStat.buffplayerStat.attackDamage * 2).ToString();
    }

    public void UISpirteSetting(ISpriteSelection playerSprite)
    {
        image[0].sprite = playerSprite.PlayerSprite;
        image[1].sprite = playerSprite.WeaponSprite;
    }
    

    
}
