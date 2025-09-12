using System;
using System.Collections;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour
{
    public void TakeAndSaveScreenshot()
    {
        StartCoroutine(SaveScreenshotCoroutine());
    }

    private IEnumerator SaveScreenshotCoroutine()
    {
        yield return new WaitForEndOfFrame();

        Texture2D screenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        screenshotTexture.ReadPixels(rect, 0, 0);
        screenshotTexture.Apply();


        byte[] imageBytes = screenshotTexture.EncodeToPNG();

        Destroy(screenshotTexture);

        string fileName = $"Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png";

#if UNITY_ANDROID && !UNITY_EDITOR
        SaveImageToGallery(imageBytes, fileName);
#else
        System.IO.File.WriteAllBytes(fileName, imageBytes);
        Debug.Log($"Screenshot saved as {fileName} in project root (Editor only).");
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void SaveImageToGallery(byte[] imageBytes, string fileName)
    {
        try
        {
            AndroidJavaClass environmentClass = new AndroidJavaClass("android.os.Environment");
            AndroidJavaClass mediaStoreClass = new AndroidJavaClass("android.provider.MediaStore$Images$Media");
            
            string relativePath = environmentClass.GetStatic<string>("DIRECTORY_DCIM") + "/Screenshots";

            AndroidJavaObject contentValues = new AndroidJavaObject("android.content.ContentValues");
            contentValues.Call("put", mediaStoreClass.GetStatic<string>("DISPLAY_NAME"), fileName);
            contentValues.Call("put", mediaStoreClass.GetStatic<string>("MIME_TYPE"), "image/png");
            contentValues.Call("put", mediaStoreClass.GetStatic<string>("RELATIVE_PATH"), relativePath);

            AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");

            AndroidJavaObject uri = contentResolver.Call<AndroidJavaObject>("insert", mediaStoreClass.GetStatic<AndroidJavaObject>("EXTERNAL_CONTENT_URI"), contentValues);

            if (uri != null)
            {
                AndroidJavaObject outputStream = contentResolver.Call<AndroidJavaObject>("openOutputStream", uri);
                
                outputStream.Call("write", imageBytes);
                outputStream.Call("flush");
                outputStream.Call("close");

                Debug.Log($"Screenshot saved successfully to DCIM/Screenshots/{fileName}");
            }
            else
            {
                Debug.LogError("Failed to create new MediaStore record.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving screenshot: {e.Message}");
        }
    }
#endif
}