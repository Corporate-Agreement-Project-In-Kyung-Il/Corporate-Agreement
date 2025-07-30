using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpGradePanel : Panel
{
    public Text failText;
    public Text doubleSucceedText;

    [System.Serializable]
    public struct UpgradeData
    {
        public MyGrade grade;
        public int chanceToSucceed;
        public int chanceToFail;
        public int chanceToDoubleSucceed;
    }
    public UpgradeData[] upgradeDatas = new  UpgradeData[5];
    private Dictionary<MyGrade, UpgradeData> m_UpgradeDatas;
    private MyGrade m_CurrentGradeIndex;
    private OptionButton m_CurrentOptionButton;
    private bool m_IsInitialized = false;
    private void Awake()
    {
        if (!m_IsInitialized)
        {
            m_UpgradeDatas = new Dictionary<MyGrade, UpgradeData>();
            foreach (var data in upgradeDatas)
            {
                m_UpgradeDatas.Add(data.grade, data);
            }
            m_IsInitialized = true;
        }
    }

    public void SetUpgradeData(MyGrade grade, OptionButton optionButton)
    {
        m_CurrentGradeIndex = grade;
        m_CurrentOptionButton = optionButton;
        contentText.text = m_UpgradeDatas[grade].chanceToSucceed.ToString();
        failText.text = m_UpgradeDatas[grade].chanceToFail.ToString();
        doubleSucceedText.text = m_UpgradeDatas[grade].chanceToDoubleSucceed.ToString();
    }

    public void TryUpgrade()
    {
        // 현재 등급의 강화 데이터 가져오기
        var upgradeData = m_UpgradeDatas[m_CurrentGradeIndex];

        // 0~99 사이의 난수
        int rand = UnityEngine.Random.Range(0, 100);

        if (rand < upgradeData.chanceToSucceed)
        {
            GameManager.Instance.UpGradeGetMatchedOptionData(m_CurrentOptionButton);
        }
        else if (rand < upgradeData.chanceToSucceed + upgradeData.chanceToDoubleSucceed)
        {
            for(int i = 0; i < 2; i++)
            {
                GameManager.Instance.UpGradeGetMatchedOptionData(m_CurrentOptionButton);
            }
        }
        else
        {
            GameManager.Instance.UpGradeFailed(m_CurrentOptionButton);
        }
    }
}
