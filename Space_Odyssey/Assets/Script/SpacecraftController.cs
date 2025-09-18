using UnityEngine;

public class SpacecraftController : MonoBehaviour
{
    [SerializeField] private Transform planet;
    [SerializeField] private float rotationSpeed = 30.0f;
    private Vector3 rotationAxis = Vector3.up;
    void Update()
    {
        if (planet != null)
        {
            transform.RotateAround(planet.position, rotationAxis, rotationSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("尚未指定行星目標！", this);
        }
    }
}
