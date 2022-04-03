using RiptideNetworking;
using UnityEngine;

namespace ClientSpecific
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform camTransform;
        private bool[] inputs;

        private void Start()
        {
            inputs = new bool[6];
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
                inputs[0] = true;
            if (Input.GetKey(KeyCode.S))
                inputs[1] = true;
            if (Input.GetKey(KeyCode.A))
                inputs[2] = true;
            if (Input.GetKey(KeyCode.D))
                inputs[3] = true;
            if (Input.GetKey(KeyCode.Space))
                inputs[4] = true;
            if (Input.GetKey(KeyCode.LeftShift))
                inputs[5] = true;
        }

        private void FixedUpdate()
        {
            sendInput();

            clearInputs();
        }

        private void clearInputs()
        {
            for (int i = 0; i < inputs.Length; i++)
                inputs[i] = false;
        }

        #region Messages
        private void sendInput()
        {
            Message message = Message.Create(MessageSendMode.unreliable, ClientToServerId.input);
            message.AddBools(inputs, false);
            message.AddVector3(camTransform.forward);
            NetworkManager.singleton.client.Send(message);
        }
        #endregion
    }
}

