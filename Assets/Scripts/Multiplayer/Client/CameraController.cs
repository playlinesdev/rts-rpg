using UnityEngine;

namespace ClientSpecific
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Player player;
        [SerializeField] private float sensitivity = 100f;
        [SerializeField] private float clampAngle = 85f;

        private float verticalRotation;
        private float horizontalRotation;

        private void OnValidate()
        {
            if (player == null)
                player = GetComponent<Player>();

        }

        private void Start()
        {
            verticalRotation = transform.localEulerAngles.x;
            horizontalRotation = player.transform.localEulerAngles.y;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                toggleCursorMode();
            if (Cursor.lockState == CursorLockMode.Locked)
                look();

            Debug.DrawRay(transform.position, transform.forward * 2f, Color.green);
        }

        private void look()
        {
            float mouseVertical = Input.GetAxis("Mouse Y");
            float mouseHorizontal = Input.GetAxis("Mouse X");

            verticalRotation += mouseVertical * sensitivity * Time.deltaTime;
            horizontalRotation += mouseHorizontal * sensitivity * Time.deltaTime;

            verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

            transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
            player.transform.rotation = Quaternion.Euler(0f, horizontalRotation, 0f);
        }

        private void toggleCursorMode()
        {
            Cursor.visible = !Cursor.visible;

            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }

}