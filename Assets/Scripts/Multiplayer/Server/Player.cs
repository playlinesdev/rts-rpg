using RiptideNetworking;
using System.Collections.Generic;
using UnityEngine;

namespace ServerSpecific
{
    public class Player : MonoBehaviour
    {
        public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

        public ushort id { get; private set; }
        public string username { get; private set; }

        [SerializeField] private PlayerMovement _movement;
        public PlayerMovement movement => _movement;

        private void OnDestroy()
        {
            list.Remove(id);
        }

        public static void spawn(ushort id, string username)
        {
            foreach (Player otherPlayer in list.Values)
                otherPlayer.sendSpawned(id);

            Player player = Instantiate(GameLogic.singleton.playerPrefab, new Vector3(0, 1, 0), Quaternion.identity).GetComponent<Player>();
            player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)})";
            player.id = id;
            player.username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username;

            player.sendSpawned();
            list.Add(id, player);
        }


        #region Messages
        private void sendSpawned()
        {
            Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawned);
            NetworkManager.singleton.server.SendToAll(addSpawnedData(message));
        }

        private void sendSpawned(ushort id)
        {
            Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawned);
            NetworkManager.singleton.server.Send(addSpawnedData(message), id);
        }

        private Message addSpawnedData(Message message)
        {
            message.AddUShort(id);
            message.AddString(username);
            message.AddVector3(transform.position);
            return message;
        }

        [MessageHandler((ushort)ClientToServerId.name)]
        private static void Name(ushort fromClientId, Message message)
        {
            spawn(fromClientId, message.GetString());
        }

        [MessageHandler((ushort)ClientToServerId.input)]
        private static void input(ushort fromClient, Message message)
        {
            if (list.TryGetValue(fromClient, out var player))
                player.movement.setInput(message.GetBools(6), message.GetVector3());
        }
        #endregion
    }
}

