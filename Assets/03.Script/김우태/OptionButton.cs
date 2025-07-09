using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OptionButton : MonoBehaviour
{
   [SerializeField]
   UnityEvent<int> m_OnClickEvent;
   
   public int selectID;
   public void OnClick()
   {
      m_OnClickEvent.Invoke(selectID);
   }
}
