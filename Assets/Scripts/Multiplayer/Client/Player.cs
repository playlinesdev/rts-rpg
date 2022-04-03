using RiptideNetworking;
using System.Collections.Generic;
using UnityEngine;

namespace ClientSpecific
{
    public class Player : MonoBehaviour
    {
        public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

        public ushort id { get; private set; }
        public bool isLocal { get; private set; }

        [SerializeField] private Transform camTransform;

        private string username;

        private void OnDestroy()
        {
            list.Remove(id);
        }

        private void move(Vector3 newPosition, Vector3 forward)
        {
            Debug.Log($"{newPosition}");
            transform.position = newPosition;
            if (!isLocal)
            {
                camTransform.forward = forward;
            }
        }

        public static void spawn(ushort id, string username, Vector3 position)
        {
            Player player;
            if (id == NetworkManager.singleton.client.Id)
            {
                player = Instantiate(GameLogic.singleton.localPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
                player.isLocal = true;
            }
            else
            {
                player = Instantiate(GameLogic.singleton.playerPrefab, position, Quaternion.identity).GetComponent<Player>();
                player.isLocal = false;
            }

            player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
            player.id = id;
            player.username = username;

            list.Add(id, player);
        }

        #region Messages
        [MessageHandler((ushort)ServerToClientId.playerSpawned)]
        private static void spawnPlayer(Message message)
        {
            spawn(message.GetUShort(), message.GetString(), message.GetVector3());
        }

        [MessageHandler((ushort)ServerToClientId.playerMovement)]
        private static void playerMovement(Message message)
        {
            if (list.TryGetValue(message.GetUShort(), out Player player))
                player.move(message.GetVector3(), message.GetVector3());
        }
        #endregion
    }
}