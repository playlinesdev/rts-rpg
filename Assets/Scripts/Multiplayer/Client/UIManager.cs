using RiptideNetworking;
using UnityEngine;
using UnityEngine.UI;

namespace ClientSpecific
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _singleton;
        public static UIManager singleton
        {
            get { return _singleton; }
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{typeof(UIManager).Name} Client network manager already exists, destroying!");
                    Destroy(value);
                }
            }
        }

        [SerializeField] private GameObject connectUI;
        [SerializeField] private InputField usernameField;
        [SerializeField] private InputField serverIpField;

        private void Awake()
        {
            singleton = this;
        }

        public void connectClicked()
        {
            usernameField.interactable = false;
            connectUI.SetActive(false);

            NetworkManager.singleton.connect(serverIpField.text);
        }

        public void backToMain()
        {
            usernameField.interactable = true;
            connectUI.SetActive(true);
        }

        public void sendName()
        {
            var message = Message.Create(MessageSendMode.reliable, ClientToServerId.name);
            message.AddString(usernameField.text);

            NetworkManager.singleton.client.Send(message);
        }
    }

}