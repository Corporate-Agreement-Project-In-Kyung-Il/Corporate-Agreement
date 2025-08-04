using UnityEngine;
using UnityEngine.UI;

public class Panel : MonoBehaviour
{
    public Text titleText;
    public Text contentText;
    public Button closeButton;
    public Button confirmButton;

    public void ReturnTitleScene()
    {
        GameManager.Instance.LoadScene(0, this.gameObject);
    }
}
