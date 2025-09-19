using UnityEngine;

public class PlanetDetector : MonoBehaviour
{
    private void OnBecameVisible()
    {
        Debug.Log(gameObject.name + " Became Visible", this.gameObject);
    }

    private void OnBecameInvisible()
    {
        Debug.Log(gameObject.name + " Became Invisible", this.gameObject);
    }

}
