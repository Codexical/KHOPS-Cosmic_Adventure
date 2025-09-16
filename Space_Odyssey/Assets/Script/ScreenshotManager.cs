using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotManager : MonoBehaviour
{
    [SerializeField] private GameObject screenshotButton;
    public void TakeScreenshot()
    {
        screenshotButton.SetActive(false);
        StartCoroutine(SaveScreenshotCoroutine());
    }

    private IEnumerator SaveScreenshotCoroutine()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenshotTexture = ScreenCapture.CaptureScreenshotAsTexture();

        string albumName = "Screenshots";
        string fileName = $"Screenshot_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";

        NativeGallery.SaveImageToGallery(
            screenshotTexture,
            albumName,
            fileName,
            (success, path) =>
            {
                if (success)
                {
                    Debug.Log("截圖成功！儲存路徑: " + path);
                }
                else
                {
                    Debug.LogError("截圖失敗！");
                }
            });
        Destroy(screenshotTexture);
        screenshotButton.SetActive(true);
    }
}