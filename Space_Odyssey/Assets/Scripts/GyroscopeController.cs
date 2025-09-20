using UnityEngine;
using System.Collections;

public class GyroscopeController : MonoBehaviour
{
    [SerializeField] GameObject perseverance;
    [SerializeField] GameObject centerPoint;
    private bool isGyroSupport;
    private Quaternion rawGyroRotation;
    private Quaternion rotationFix = Quaternion.Euler(90f, -90f, 0f);
    private Quaternion gyroRotation;
    private float smoothSpeed = 5.0f;
    private bool atMars = false;

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
        if (isGyroSupport && !atMars)
        {
            rawGyroRotation = Input.gyro.attitude;
            Quaternion convertedRotation = new Quaternion(rawGyroRotation.x, rawGyroRotation.y, -rawGyroRotation.z, -rawGyroRotation.w);
            gyroRotation = rotationFix * convertedRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, gyroRotation, 0.2f);
        }
    }

    public void goToMars()
    {
        atMars = true;
        StartCoroutine(MoveToTarget(perseverance.transform, 3.0f));
    }

    public void resetGyro()
    {
        atMars = false;
        StartCoroutine(MoveToCenter(2.0f));
    }

    private IEnumerator MoveToTarget(Transform target, float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = target.position;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = target.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        transform.position = endPosition;
        transform.rotation = endRotation;
        Debug.Log("Arrived at Perseverance!");
    }

    private IEnumerator MoveToCenter(float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = centerPoint.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        transform.position = endPosition;
        Debug.Log("Returned to Center Point!");
    }

}