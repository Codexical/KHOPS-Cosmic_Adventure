using UnityEngine;

public class PlanetDetector : MonoBehaviour
{
    [SerializeField] private GameObject backButton;
    [SerializeField] private GyroscopeController gyroscopeController;
    [SerializeField] private float rotationSpeedY = 10.0f;
    [SerializeField] private float rotationSpeedZ = 10.0f;

    private bool atMars = false;
    private void OnMouseDown()
    {
        if (atMars) return;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        atMars = true;
        Debug.Log("Planet clicked: " + gameObject.name);
        backButton.SetActive(true);
        gyroscopeController.goToMars();
    }

    public void ResetPosition()
    {
        if (!atMars) return;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        atMars = false;
        backButton.SetActive(false);
        gyroscopeController.resetGyro();
    }

    void Start()
    {
        backButton.SetActive(false);
    }

    void Update()
    {
        if (atMars)
        {
            Vector3 rotationVector = new Vector3(0, 0, -rotationSpeedZ);
            transform.Rotate(rotationVector * Time.deltaTime, Space.Self);
        }
        else
        {
            Vector3 rotationVector = new Vector3(0, rotationSpeedY, 0);
            transform.Rotate(rotationVector * Time.deltaTime, Space.Self);
        }
    }
}
