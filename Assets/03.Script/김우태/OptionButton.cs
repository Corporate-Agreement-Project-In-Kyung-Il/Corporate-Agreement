using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
    public Button selectionButton;
    public Button enchantButton;
    
    [Serializable]
    public class ButtonEvent : UnityEvent<GameObject> { }

    [SerializeField]
    [Tooltip("Events to fire when the button is clicked.")]
    ButtonEvent m_OnClick = new ButtonEvent();

    [SerializeField]
    [Tooltip("UI Button component")]
    Button button;  // UI Button
    
    // Events to fire when the button is clicked.
    public ButtonEvent onClick => m_OnClick;

    void Start()
    {
        // 버튼이 클릭되었을 때 호출되는 이벤트 등록
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClickEvent);
        }
        else
        {
            Debug.LogWarning("Button is not assigned.");
        }
    }

    void OnButtonClickEvent()
    {
        m_OnClick?.Invoke(gameObject);
    }
}
