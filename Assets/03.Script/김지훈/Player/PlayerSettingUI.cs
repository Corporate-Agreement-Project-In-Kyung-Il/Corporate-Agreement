using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingUI : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;
    [SerializeField] private Image panel;
    [SerializeField] private TextMeshProUGUI text;
    private Button button;
    private bool isInformationDisplayed = false;
    [SerializeField] private PlayerInformationGraphic informationSetting;

    private PlayerInformationGraphic[] PlayerArray = new PlayerInformationGraphic[3];
    private void Start()
    {
        TryGetComponent(out button);
        button.onClick.AddListener(PlayerSettingClicked);
    }

    private void PlayerSettingClicked()
    {
        if (isInformationDisplayed)
        {
            for (int i = 0; i < PlayerArray.Length; i++)
                Destroy(PlayerArray[i].gameObject);
            panel.gameObject.SetActive(false);
            isInformationDisplayed = false;
            text.text = "현재 상태";
        }
        else
        {
            var player = AliveExistSystem.Instance.playerList;
            panel.gameObject.SetActive(true);
            for (int i = 0; i < player.Count; i++)
            {
                var info = Instantiate(informationSetting, informationSetting.transform.position,
                    Quaternion.identity, parentTransform);

                if (player[i].gameObject.TryGetComponent(out IBuffSelection playerStat))
                {
                    if (playerStat.buffplayerStat.isDead)
                        return;

                    info.UIPlayerInformationSetting(playerStat);
                }

                if (player[i].gameObject.TryGetComponent(out ISpriteSelection playerSprite))
                {
                    info.UISpirteSetting(playerSprite);
                    PlayerArray[i] = info;
                }
            }

            isInformationDisplayed = true;
            text.text = "뒤로가기";
        }

    }
}
