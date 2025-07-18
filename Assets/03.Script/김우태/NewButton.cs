using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // 클릭 / 꾹 누르기 
    public float holdThreshold = 0.5f;
    private bool m_IsPointerDown = false;
    private float m_PointerDownTime = 0.0f;
    
    private void Update()
    {
        if (m_IsPointerDown)
        {
            m_PointerDownTime += Time.deltaTime;
            if (m_PointerDownTime > holdThreshold)
            {
                OnHold();
            }
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        m_IsPointerDown = true;
        m_PointerDownTime = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (m_PointerDownTime < holdThreshold)
        {
            OnClick(); // 짧은 클릭 동작
        }

        m_IsPointerDown = false;
        m_PointerDownTime = 0f;
    }

    private void OnHold()
    {
        Debug.Log("꾹 누르기!");
    }

    private void OnClick()
    {
        Debug.Log("클릭!");
    }
}