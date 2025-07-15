using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuffCanvasManager : MonoBehaviour
{
    [SerializeField] private List<BuffSO> BuffList = new List<BuffSO>();
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private GameObject parentPanel;
    [SerializeField] private GameObject buffIconPrefab;

    private void Start()
    {
        // 기존 버프 리스트 받아오기
        BuffList.Clear();
        BuffList.AddRange(skillManager.buffs);

        UpdateBuffIcons();
    }

    private void UpdateBuffIcons()
    {
        // 1. 기존 아이콘 전부 삭제
        foreach (Transform child in parentPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // 2. 새로 생성
        foreach (var buff in BuffList)
        {
            SpriteRenderer sr = buff.SkillPrefab.GetComponentInChildren<SpriteRenderer>();
            if (sr == null) continue;

            GameObject icon = Instantiate(buffIconPrefab, parentPanel.transform);
            Image img = icon.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = sr.sprite;
            }
        }
    }
}