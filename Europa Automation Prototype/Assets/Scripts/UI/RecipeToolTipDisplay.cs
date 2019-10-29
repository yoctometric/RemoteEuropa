using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
public class RecipeToolTipDisplay : MonoBehaviour
{
    [SerializeField] Color BGC;
    [SerializeField] Image im2;
    [SerializeField] TMP_Text tmText;
    [SerializeField] TMP_Text defaultTextTemplate;

    [SerializeField] int imageWandH;
    Image img;
    Camera cam;
    GameObject parent;
    private void Start()
    {
        cam = Camera.main;
        img = gameObject.GetComponent<Image>();
        UnsetToolTip();
    }

    public void UpdateToolTip(List<Sprite> images, List<string> imageString, List<Color> imgColors, string mainText)
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
        im2.color = new Color(im2.color.r, im2.color.g, im2.color.b, 1);
        parent = new GameObject("parentOfImages");
        parent.transform.SetParent(transform);
        TMP_Text t = null;
        for (int i = 0; i < images.Count; i++)
        {
            //create an image
            Image im = new GameObject("image " + i.ToString()).AddComponent<Image>();
            im.transform.SetParent(parent.transform);
            im.preserveAspect = true;
            //set parent and sprite and color
            im.sprite = images[i];
            im.color = imgColors[i];
            //scale image based on origional sprite scale
            im.rectTransform.sizeDelta = new Vector2(imageWandH, imageWandH);
            im.rectTransform.position = new Vector3(transform.position.x - imageWandH - 5, transform.position.y + (imageWandH * i) + (imageWandH / 2) + tmText.fontSize + 5, transform.position.z);
            //set the text up
            t = Instantiate(defaultTextTemplate, im.rectTransform);
            t.text = imageString[i];
            //make the tmptext's box like super long so that it is never a problem
            float boxWidth = 1000;
            float boxOffset = boxWidth / 2;
            t.rectTransform.position = new Vector2(im.rectTransform.position.x - (3.5f * imageWandH), im.rectTransform.position.y);
            t.alignment = TextAlignmentOptions.Right;
        }
        //get longest string in list to find the width of the image
        List<string> sortedStrings = imageString.OrderBy(n => n.Length).ToList();
        //resize bg image
        //first, set the main text
        tmText.text = mainText;
        //calculate the width of the images and strings, and the L of the text width
        
        //string longestInfoString = imageString.Max();
        float mainTextWidth = tmText.text.Length * tmText.fontSize / 2;
        float bodyWidth = sortedStrings[sortedStrings.Count - 1].Length * t.fontSize / 2 + (imageWandH * 2.5f);
        //choose the larger one
        float biggestWidth = Mathf.Max(mainTextWidth, bodyWidth);
        Vector2 size = new Vector2(biggestWidth,(images.Count * imageWandH) + 10 + tmText.fontSize);
        //set sizes
        im2.rectTransform.sizeDelta = size;
        img.rectTransform.sizeDelta = new Vector2(size.x + 5, size.y + 5);

    }

    public void UnsetToolTip()
    {
        if (parent)
        {
            Destroy(parent);
        }
        tmText.text = "";
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        im2.color = new Color(im2.color.r, im2.color.g, im2.color.b, 0);


    }
    void Update()
    {
        Vector3 pos = Input.mousePosition + new Vector3(-10, 10, 0);
        pos = new Vector2(Mathf.Clamp(pos.x, img.rectTransform.sizeDelta.x, Screen.width), Mathf.Clamp(pos.y, 0, Screen.height - img.rectTransform.sizeDelta.y));//an attempt to clamp the tooltip
        transform.position = pos;
    }
}
