using RiptideNetworking;
using UnityEngine;

namespace ServerSpecific
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private CharacterController controller;
        [SerializeField] private Transform camProxy;
        [SerializeField] private float gravity;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float jumpHeight;

        private float gravityAcceleration;
        private float moveSpeed = 0.05f;
        private float jumpSpeed;

        private bool[] inputs;
        private float yVelocity;

        private void OnValidate()
        {
            if (controller == null)
                controller = GetComponent<CharacterController>();
            if (player == null)
                player = GetComponent<Player>();
            initialize();
        }

        private void Start()
        {
            inputs = new bool[6];
        }

        private void FixedUpdate()
        {
            Vector2 inputDirection = Vector2.zero;
            if (inputs[0])
                inputDirection.y += 1;
            if (inputs[1])
                inputDirection.y -= 1;
            if (inputs[2])
                inputDirection.x -= 1;
            if (inputs[3])
                inputDirection.x += 1;

            move(inputDirection, inputs[4], inputs[5]);
        }

        private void initialize()
        {
            gravityAcceleration = gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
            //moveSpeed = movementSpeed * Time.fixedDeltaTime;
            jumpSpeed = Mathf.Sqrt(jumpHeight * -2f * gravityAcceleration);
        }

        private void move(Vector2 inputDirection, bool jump, bool sprint)
        {
            Vector3 moveDirection = Vector3.Normalize(camProxy.right * inputDirection.x + Vector3.Normalize(flattenVector(camProxy.forward)) * inputDirection.y);
            Debug.Log($"moveSpeed: {moveSpeed}");
            moveDirection *= moveSpeed;

            if (sprint)
                moveDirection *= 2;

            if (controller.isGrounded)
            {
                yVelocity = 0;
                if (jump)
                    yVelocity = jumpSpeed;
            }

            yVelocity *= gravityAcceleration;

            moveDirection.y = yVelocity;
            controller.Move(moveDirection);

            sendMovement();
        }

        private Vector3 flattenVector(Vector3 vector)
        {
            vector.y = 0;
            return vector;
        }

        public void setInput(bool[] inputs, Vector3 forward)
        {
            this.inputs = inputs;
            camProxy.forward = forward;
        }

        public void sendMovement()
        {
            Message message = Message.Create(MessageSendMode.unreliable, ServerToClientId.playerMovement);
            message.AddUShort(player.id);
            message.AddVector3(transform.position);
            message.AddVector3(camProxy.forward);

            NetworkManager.singleton.server.SendToAll(message);
        }
    }
}