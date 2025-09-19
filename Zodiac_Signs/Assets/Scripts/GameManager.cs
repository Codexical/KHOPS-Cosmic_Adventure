using UnityEngine;

[System.Serializable]
public class Config
{
    public string apiUrl;
    public string port;
    public string type;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _server;
    [SerializeField] private GameObject _client;
    [SerializeField] private API _api;
    private string configPath = "./config.json";
    public Config config = null;
    void Start()
    {
        LoadConfig();
    }

    void Update()
    {

    }

    public void LoadConfig()
    {
        if (!System.IO.File.Exists(configPath))
        {
            Config defaultConfig = new Config { apiUrl = "http://127.0.0.1", port = "5000", type = "Server" };
            string defaultJson = JsonUtility.ToJson(defaultConfig, true);
            System.IO.File.WriteAllText(configPath, defaultJson);
        }
        config = JsonUtility.FromJson<Config>(System.IO.File.ReadAllText(configPath));
        _api.apiUrl = config.apiUrl;
        _api.portName = config.port;
        if (config.type == "Server")
        {
            _server.SetActive(true);
            _client.SetActive(false);
        }
        else
        {
            _server.SetActive(false);
            _client.SetActive(true);
        }
    }
}
