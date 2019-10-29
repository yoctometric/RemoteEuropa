using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ToolTipActivator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    ToolTipDisplay tT;
    RecipeToolTipDisplay rTT;
    bool mouseOn = false;
    [SerializeField] string toolTipText;
    [SerializeField] bool isRecipeDisplay = false;

    [SerializeField] List<Color> imgColors;
    [SerializeField] List<Sprite> images;
    [SerializeField] List<string> imgDescs;

    void Start()
    {
        tT = GameObject.FindObjectOfType<ToolTipDisplay>();
        rTT = GameObject.FindObjectOfType<RecipeToolTipDisplay>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isRecipeDisplay)
        {
            rTT.UpdateToolTip(images, imgDescs, imgColors, toolTipText);
        }
        else
        {
            tT.SetToolTip(toolTipText);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isRecipeDisplay)
        {
            rTT.UnsetToolTip();
        }
        else
        {
            tT.UnSetToolTip();
        }
    }
}
