using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    public Text titleText;
    public Text contentText;
    public Button closeButton;
    public Button confirmButton;

    public void ReturnTitleScene()
    {
        SceneManager.LoadScene(0);
    }
}
