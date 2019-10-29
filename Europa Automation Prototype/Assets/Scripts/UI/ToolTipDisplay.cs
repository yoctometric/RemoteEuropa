using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToolTipDisplay : MonoBehaviour
{
    [SerializeField] Color BGC;
    [SerializeField] Image im2;
    [SerializeField] TMP_Text tmText;
    Image img;
    Camera cam;
    private void Start()
    {

        cam = Camera.main;
        img = gameObject.GetComponent<Image>();
        //SetToolTip("reallllllyyyyyyyyyyyyyyyyyyyyy long message");
        UnSetToolTip();
    }
    private void Update()
    {
        Vector3 pos = Input.mousePosition + new Vector3(-10, 10, 0);
        pos = new Vector2(Mathf.Clamp(pos.x, img.rectTransform.sizeDelta.x, Screen.width), Mathf.Clamp(pos.y, 0, Screen.height - img.rectTransform.sizeDelta.y));//an attempt to clamp the tooltip
        transform.position = pos;
        Vector2 size = new Vector2(tmText.text.Length * tmText.fontSize / 2, tmText.fontSize);
        im2.rectTransform.sizeDelta = size;
        img.rectTransform.sizeDelta = new Vector2(size.x + 5, size.y + 5);
    }
    public void SetToolTip(string t)
    {
        tmText.text = t;
        img.color = new Color(BGC.r, BGC.g, BGC.b, 1);
    }
    public void UnSetToolTip()
    {
        tmText.text = "";
        img.color = new Color(BGC.r, BGC.g, BGC.b, 0);
    }
}
