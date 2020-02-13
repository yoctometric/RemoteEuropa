using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SliderFamily : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Image fillArea;
    [HideInInspector]public Slider slider;
    Color modifyThisColor = new Color(1f, 0.1f, 0.1f, 1f);
    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }
    public void SetSliderParameters(int max)
    {
        slider.maxValue = max;
    }
    public void ChangeValue(int val = -1)
    {
        if (!slider)
        {
            slider = gameObject.GetComponent<Slider>();
        }
        if(val != -1)
        {
            slider.value = val;//this val is default to -1, usually slider is set thru other ways
        }

        text.text = slider.value.ToString();
        float colorVal = slider.normalizedValue;
        modifyThisColor.r = 1 - colorVal;
        modifyThisColor.g = colorVal;
        fillArea.color = modifyThisColor;
    }
}
