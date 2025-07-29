using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInformationGraphic : MonoBehaviour
{
    [SerializeField] private TMP_Text[] text;
    [SerializeField] private Image[] image;
    
    public void UIPlayerInformationSetting(IBuffSelection playerStat)
    {
        text[0].text = playerStat.buffplayerStat.characterName.ToString(CultureInfo.InvariantCulture);
        text[1].text = playerStat.buffplayerStat.characterClass.ToString(CultureInfo.InvariantCulture);
        text[2].text = playerStat.buffplayerStat.characterGrade.ToString(CultureInfo.InvariantCulture);
        text[3].text = playerStat.buffplayerStat.health.ToString(CultureInfo.InvariantCulture);
        text[4].text = playerStat.buffplayerStat.attackDamage.ToString(CultureInfo.InvariantCulture);
        text[5].text = playerStat.buffplayerStat.attackSpeed.ToString(CultureInfo.InvariantCulture);
        text[6].text = playerStat.buffplayerStat.equip_level.ToString(CultureInfo.InvariantCulture);
        text[7].text = playerStat.buffplayerStat.criticalProbability.ToString(CultureInfo.InvariantCulture);
    }

    public void UISpirteSetting(ISpriteSelection playerSprite)
    {
        image[0].sprite = playerSprite.PlayerSprite;
        image[1].sprite = playerSprite.WeaponSprite;
    }
    
}
