using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EsekaiClickButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image image;
    public void SetSpriteFromButton(Button clickedButton)
    {
        Image buttonImage = clickedButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            image.sprite = buttonImage.sprite;
        }
        else
        {
            Debug.LogWarning("해당 버튼에 Image 컴포넌트가 없습니다.");
        }
    }
}
