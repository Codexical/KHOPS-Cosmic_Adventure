using UnityEngine;

public class StartManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;

    public void StartGame()
    {
        startPanel.SetActive(false);
    }
}
