using RiptideNetworking;
using RiptideNetworking.Utils;
using System;
using UnityEngine;

namespace ClientSpecific
{
    public class NetworkManager : MonoBehaviour
    {
        private static NetworkManager _singleton;
        public static NetworkManager singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{typeof(NetworkManager)} Client network manager already exists, destroying!");
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
            client.ClientDisconnected += playerLeft;
            client.Disconnected += didDisconnect;
        }

        private void FixedUpdate()
        {
            client.Tick();
        }

        private void OnApplicationQuit()
        {
            client.Disconnect();
        }

        private void playerLeft(object sender, ClientDisconnectedEventArgs e)
        {
            Destroy(Player.list[e.Id].gameObject);
        }

        public void connect(string srvIp)
        {
            client.Connect($"{(string.IsNullOrEmpty(srvIp) ? serverIp : srvIp) }:{serverPort}");
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
}

