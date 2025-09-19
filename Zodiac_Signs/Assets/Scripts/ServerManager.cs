using UnityEngine;
using TMPro;

public class ServerManager : MonoBehaviour
{
    [SerializeField] private API _api;
    [SerializeField] private TextMeshProUGUI[] _astronomicalTexts;
    [SerializeField] private TextMeshProUGUI[] _astrologicalTexts;

    private void Start()
    {

    }

    void Update()
    {
        if (_api != null && _astronomicalTexts != null && _astrologicalTexts != null)
        {
            for (int i = 0; i < _astronomicalTexts.Length && i < _api.astronomicalCounts.Length; i++)
            {
                _astronomicalTexts[i].text = _api.astronomicalCounts[i].ToString();
            }
            for (int i = 0; i < _astrologicalTexts.Length && i < _api.astrologicalCounts.Length; i++)
            {
                _astrologicalTexts[i].text = _api.astrologicalCounts[i].ToString();
            }
        }
    }
}
