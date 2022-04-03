using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

namespace ServerSpecific
{
    public class NetworkManager : MonoBehaviour
    {
        private static NetworkManager _singleton;
        public static NetworkManager singleton
        {
            get { return _singleton; }
            set
            {
                if (_singleton == null)
                {
                    _singleton = value;
                }
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                    Destroy(value);
                }
            }
        }

        public Server server { get; private set; }

        [SerializeField] private ushort port;
        [SerializeField] private ushort maxClientCount;

        private void Awake()
        {
            singleton = this;
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

            server = new Server();
            server.Start(port, maxClientCount);
            server.ClientDisconnected += playerLeft;
        }

        private void FixedUpdate()
        {
            server.Tick();
        }

        private void OnApplicationQuit()
        {
            server.Stop();
        }

        private void playerLeft(object sender, ClientDisconnectedEventArgs e)
        {
            if (Player.list.TryGetValue(e.Id, out Player player))
                Destroy(player.gameObject);
        }
    }

}
