using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class CameraController : MonoBehaviour
{
    public RawImage background;
    public AspectRatioFitter aspectRatioFitter;

    private WebCamTexture cameraTexture;

    IEnumerator Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            yield return new WaitForSeconds(1); 
        }
#endif

        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            InitializeCamera();
        }
        else
        {
            Debug.LogError("User denied camera permission.");
        }
    }

    private void InitializeCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.LogError("Cannot find any camera device.");
            return;
        }

        string backCameraName = "";
        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCameraName = devices[i].name;
                break;
            }
        }

        if (string.IsNullOrEmpty(backCameraName))
        {
            Debug.LogWarning("Cannot find back camera, using front camera instead.");
            backCameraName = devices[0].name;
        }

        cameraTexture = new WebCamTexture(backCameraName, 3264, 2032);
        cameraTexture.Play();

        background.texture = cameraTexture;

        StartCoroutine(FixAspectRatio());
    }

    IEnumerator FixAspectRatio()
    {
        yield return new WaitUntil(() => cameraTexture.width > 100);

        Debug.Log($"Camera Resolution: {cameraTexture.width}x{cameraTexture.height}");

        float ratio = (float)cameraTexture.width / (float)cameraTexture.height;
        aspectRatioFitter.aspectRatio = ratio;

        if (cameraTexture.videoVerticallyMirrored)
        {
            background.uvRect = new Rect(1, 0, -1, 1);
        }
        else
        {
            background.uvRect = new Rect(0, 0, 1, 1);
            background.transform.localEulerAngles = new Vector3(0, 0, -cameraTexture.videoRotationAngle);
        }

        void OnDestroy()
        {
            if (cameraTexture != null && cameraTexture.isPlaying)
            {
                cameraTexture.Stop();
            }
        }
    }
}