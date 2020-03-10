using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SliderInputFamily : MonoBehaviour
{
    TMP_InputField input;
    [HideInInspector] public Slider slider;
    [HideInInspector] public float storedVal = 0;
    [SerializeField] List<char> validChars;
    float minVal = 0;
    float maxVal = 0;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        minVal = slider.minValue;
        maxVal = slider.maxValue;
        input = GetComponentInChildren<TMP_InputField>();
        storedVal = slider.value;
        input.text = storedVal.ToString();
    }
    public void UpdateValuesFromSlider()
    {
        storedVal = slider.value;
        input.text = (Mathf.Round(storedVal * 100f) / 100f).ToString();
    }
    public void UpdateValuesFromInput()
    {
        string inputVal = input.text;

        for (int i = 0; i < inputVal.Length; i++)
        {
            if (!validChars.Contains(inputVal[i]))
            {
                //print("INVALID INPUT!");
                return;
            }
        }
        if (input.text.Length > 0)
        {
            float val = float.Parse(input.text);
            if (val > maxVal)
            {
                storedVal = maxVal;
            }
            else if (val < minVal)
            {
                storedVal = minVal;
            }
            else
            {
                storedVal = val;
            }
        }
        input.text = storedVal.ToString();
        slider.value = storedVal;
        //print(storedVal);
    }
    public void UpdateValuesFromOutsideSource(float val)
    {
        storedVal = val;
        input.text = (Mathf.Round(storedVal * 100f) / 100f).ToString();
        slider.value = storedVal;
    }
}
