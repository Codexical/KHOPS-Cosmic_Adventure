using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class API : MonoBehaviour
{
    public string apiUrl = "http://127.0.0.1";
    public string portName = "5000";
    public int[] astrologicalCounts = new int[12];
    public int[] astronomicalCounts = new int[13];

    private class CountsResponse
    {
        public List<int> ASTROLOGICAL;
        public List<int> ASTRONOMICAL;
    }

    void Start()
    {
        InvokeRepeating("GetAllCountsRequest", 0f, 1f);
    }

    public void GetAllCountsRequest()
    {
        Debug.Log("Requesting counts from server...");
        StartCoroutine(GetAllCounts());
    }

    private IEnumerator GetAllCounts()
    {
        string url = $"{apiUrl}:{portName}/counts";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                // 解析JSON
                string json = request.downloadHandler.text;
                CountsResponse response = JsonUtility.FromJson<CountsResponse>(json);
                if (response != null)
                {
                    astrologicalCounts = response.ASTROLOGICAL.ToArray();
                    astronomicalCounts = response.ASTRONOMICAL.ToArray();
                    // Debug.Log("ASTROLOGICAL: " + string.Join(",", astrologicalCounts));
                    // Debug.Log("ASTRONOMICAL: " + string.Join(",", astronomicalCounts));
                }
                else
                {
                    Debug.LogError("JSON parse error: " + json);
                }
            }
        }
    }
}

