using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
public class MiniMap : MonoBehaviour
{
    [SerializeField] GameObject img;
    [SerializeField] TMP_Text text;

    Camera cam;

    bool open = false;
    string defaultDisp;

    private void Start()
    {
        defaultDisp = text.text;
        cam = GameObject.FindGameObjectWithTag("MapCamera").GetComponent<Camera>();
    }

    public void ToggleExpand()
    {
        open = !open;
        img.SetActive(open);
    }

    public void TakeScreenShot()
    {
        //ScreenCapture.CaptureScreenshot(Application.dataPath + "/screenshots/" + "EurScreen " + Time.time, 2);
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;

        cam.Render();

        Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;

        byte[] bytes = image.EncodeToPNG();
        Destroy(image);

        File.WriteAllBytes(Application.dataPath + "/screenshots/EurScreen " + Time.time + ".png", bytes);
        StartCoroutine(TakenScreenShot());

    }

    IEnumerator TakenScreenShot()
    {
        text.alpha = 1;
        while(text.alpha > 0)
        {
            yield return new WaitForSeconds(0.01f);
            text.alpha -= 0.01f;
        }
    }


    public void OpenScreenshotsFolder()
    {
        if (OpenInFileBrowser.IsInMacOS)
        {
            OpenInFileBrowser.OpenInMac(Application.dataPath + "/screenshots/");
        }else if (OpenInFileBrowser.IsInWinOS)
        {
            OpenInFileBrowser.OpenInWin(Application.dataPath + "/screenshots/");
        }
        else
        {
            //linux is not supported. Rip
            StartCoroutine(RenderError("Unable to perform operation on this operating system. Sorry"));
        }
    }

    IEnumerator RenderError(string err)
    {
        text.alpha = 1;
        text.text = err;
        while (text.alpha > 0)
        {
            yield return new WaitForSeconds(0.01f);
            text.alpha -= 0.005f;
        }
        text.text = defaultDisp;
    }
}
