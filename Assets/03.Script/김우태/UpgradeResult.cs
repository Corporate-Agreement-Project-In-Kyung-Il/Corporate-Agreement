using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeResult : MonoBehaviour
{
    public Text upgradeResultText;
    public Image upgradeImage;
    public Text upgradeContentText;
    
    public void CloseResultPanel()
    {
        gameObject.SetActive(false);
    }
}
