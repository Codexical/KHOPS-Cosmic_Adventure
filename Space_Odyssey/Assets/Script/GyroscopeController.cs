using UnityEngine;

public class GyroscopeController : MonoBehaviour
{
    private bool isGyroSupport;
    private Quaternion rawGyroRotation;
    private Quaternion rotationFix = Quaternion.Euler(90f, -90f, 0f);
    private Quaternion gyroRotation;
    private float smoothSpeed = 5.0f;

    void Start()
    {
        isGyroSupport = SystemInfo.supportsGyroscope;
        if (isGyroSupport)
        {
            Input.gyro.enabled = true;
        }
        else
        {
            Debug.Log("Gyroscope not supported on this device");
        }
    }

    void Update()
    {
        if (isGyroSupport)
        {
            rawGyroRotation = Input.gyro.attitude;
            Quaternion convertedRotation = new Quaternion(rawGyroRotation.x, rawGyroRotation.y, -rawGyroRotation.z, -rawGyroRotation.w);
            gyroRotation = rotationFix * convertedRotation;
            transform.rotation = gyroRotation;
            Debug.Log(transform.rotation);
        }
    }
}