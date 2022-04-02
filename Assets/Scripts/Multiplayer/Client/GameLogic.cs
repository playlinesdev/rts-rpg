using UnityEngine;

namespace ClientSpecific
{
    public class GameLogic : MonoBehaviour
    {
        private static GameLogic _singleton;
        public static GameLogic singleton
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
                    Debug.Log($"{nameof(GameLogic)} instance already exists, destroying duplicate!");
                    Destroy(value);
                }
            }
        }

        public GameObject playerPrefab => _playerPrefab;
        public GameObject localPlayerPrefab => _localPlayerPrefab;

        [Header("Prefabs")]
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _localPlayerPrefab;

        private void Awake()
        {
            singleton = this;
        }
    }

}