using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SliderFamily : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Image fillArea;
    Slider slider;
    Color modifyThisColor = new Color(1f, 0.1f, 0.1f, 1f);

    private void Start()
    {
        slider = gameObject.GetComponent<Slider>();
    }
    public void OnValueChanged()
    {
        text.text = slider.value.ToString();
        float colorVal = slider.normalizedValue;
        modifyThisColor.r = 1 - colorVal;
        modifyThisColor.g = colorVal;
        fillArea.color = modifyThisColor;
    }
}
