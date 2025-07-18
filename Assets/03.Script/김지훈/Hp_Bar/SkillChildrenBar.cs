using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillChildrenBar : MonoBehaviour
{
    public Slider Slider => slider;
    private Slider slider;

    private void Start()
    {
        TryGetComponent(out slider);
    }
}
