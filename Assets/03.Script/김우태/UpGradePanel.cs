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
        public EMyGrade grade;
        public int chanceToSucceed;
        public int chanceToFail;
        public int chanceToDoubleSucceed;
    }
    public UpgradeData[] upgradeDatas = new  UpgradeData[5];
    
    public UpgradeResult upgradeSuccessPanel;
    public UpgradeResult upgradeFailPanel;
    public GameObject upgradeAnimationObject;
    public Animator upgradeAnimation;
    private Dictionary<EMyGrade, UpgradeData> m_UpgradeDatas;
    private EMyGrade m_CurrentGradeIndex;
    private OptionButton m_CurrentOptionButton;
    private bool m_IsInitialized = false;
    private void Awake()
    {
        if (!m_IsInitialized)
        {
            m_UpgradeDatas = new Dictionary<EMyGrade, UpgradeData>();
            foreach (var data in upgradeDatas)
            {
                m_UpgradeDatas.Add(data.grade, data);
            }
            m_IsInitialized = true;
        }
    }

    public void SetUpgradeData(EMyGrade grade, OptionButton optionButton)
    {
        m_CurrentGradeIndex = grade;
        m_CurrentOptionButton = optionButton;
        contentText.text = m_UpgradeDatas[grade].chanceToSucceed.ToString();
        failText.text = m_UpgradeDatas[grade].chanceToFail.ToString();
        doubleSucceedText.text = m_UpgradeDatas[grade].chanceToDoubleSucceed.ToString();
    }

    IEnumerator WaitForAnimationEnd()
    {
        upgradeAnimation.gameObject.SetActive(true);
        while (true)
        {
            AnimatorStateInfo info = upgradeAnimation.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime >= 1f && !upgradeAnimation.IsInTransition(0))
            {
                upgradeAnimation.gameObject.SetActive(false);
                upgradeAnimationObject.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }

        Debug.Log("애니메이션 끝!");
    }
    
    public void TryUpgrade()
    {
        if (m_CurrentOptionButton.isUpgradable == false)
        {
            return;
        }
        upgradeAnimationObject.gameObject.SetActive(true);
        StartCoroutine(WaitForAnimationEnd());
        // 현재 등급의 강화 데이터 가져오기
        var upgradeData = m_UpgradeDatas[m_CurrentGradeIndex];

        // 0~99 사이의 난수
        int rand = UnityEngine.Random.Range(0, 100);

        if (rand < upgradeData.chanceToSucceed)
        {
            GameManager.Instance.UpGradeGetMatchedOptionData(m_CurrentOptionButton);
            upgradeSuccessPanel.gameObject.SetActive(true);
            upgradeSuccessPanel.upgradeResultText.text = "강화 성공";
            upgradeSuccessPanel.upgradeImage.sprite = m_CurrentOptionButton.choiceImage.sprite;
            upgradeSuccessPanel.upgradeContentText.text = m_CurrentOptionButton.selectedData.GetGrade().ToString();
        }
        else if (rand < upgradeData.chanceToSucceed + upgradeData.chanceToDoubleSucceed)
        {
            for(int i = 0; i < 2; i++)
            {
                GameManager.Instance.UpGradeGetMatchedOptionData(m_CurrentOptionButton);
                upgradeSuccessPanel.gameObject.SetActive(true);
                upgradeSuccessPanel.upgradeResultText.text = "강화 대성공";
                upgradeSuccessPanel.upgradeImage.sprite = m_CurrentOptionButton.choiceImage.sprite;
                upgradeSuccessPanel.upgradeContentText.text = m_CurrentOptionButton.selectedData.GetGrade().ToString();
            }
        }
        else
        {
            upgradeFailPanel.gameObject.SetActive(true);
            upgradeFailPanel.upgradeResultText.text = "강화 실패";
            upgradeFailPanel.upgradeImage.sprite = m_CurrentOptionButton.choiceImage.sprite;
            upgradeFailPanel.upgradeContentText.text = m_CurrentOptionButton.selectedData.GetGrade().ToString();
            m_CurrentOptionButton.isUpgradable = false;
            m_CurrentOptionButton.brokenImage.SetActive(true);
            //closeButton.onClick.Invoke();
        }
    }
}
