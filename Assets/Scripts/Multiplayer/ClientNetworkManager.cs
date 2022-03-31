using RiptideNetworking;
using RiptideNetworking.Utils;
using System;
using UnityEngine;

public enum ClientToServerId : ushort
{
    name = 1,
}

public class ClientNetworkManager : MonoBehaviour
{
    private static ClientNetworkManager _singleton;
    public static ClientNetworkManager singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{typeof(ClientNetworkManager).Name} Client network manager already exists, destroying!");
                Destroy(value);
            }
        }
    }

    public Client client { get; private set; }

    [SerializeField] private string serverIp;
    [SerializeField] private ushort serverPort;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        client = new Client();
        client.Connected += didConnect;
        client.ConnectionFailed += failedToConnect;
        client.ClientDisconnected += didDisconnect;
    }

    public void connect()
    {
        client.Connect($"{serverIp}:{serverPort}");
    }

    private void FixedUpdate()
    {
        client.Tick();
    }

    private void OnApplicationQuit()
    {
        client.Disconnect();
    }

    private void didConnect(object sender, EventArgs e)
    {
        UIManager.singleton.sendName();
    }

    private void failedToConnect(object sender, EventArgs e)
    {
        UIManager.singleton.backToMain();
    }

    private void didDisconnect(object sender, EventArgs e)
    {
        UIManager.singleton.backToMain();
    }
}
