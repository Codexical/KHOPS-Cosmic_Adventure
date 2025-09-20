using UnityEngine;

public class SpinWheel : MonoBehaviour
{
    [SerializeField] private GameObject[] rightWheels;
    [SerializeField] private GameObject[] leftWheels;
    [SerializeField] private float rotationSpeed = 10.0f;

    void Update()
    {
        foreach (var wheel in rightWheels)
        {
            Vector3 rotationVector = new Vector3(rotationSpeed, 0, 0);
            wheel.transform.Rotate(rotationVector * Time.deltaTime, Space.Self);
        }

        foreach (var wheel in leftWheels)
        {
            Vector3 rotationVector = new Vector3(-rotationSpeed, 0, 0);
            wheel.transform.Rotate(rotationVector * Time.deltaTime, Space.Self);
        }
    }
}
