using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeedY = 10.0f;

    void Update()
    {
        Vector3 rotationVector = new Vector3(0, rotationSpeedY, 0);
        transform.Rotate(rotationVector * Time.deltaTime, Space.Self);
    }
}