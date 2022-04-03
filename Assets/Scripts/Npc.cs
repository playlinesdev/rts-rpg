using UnityEngine;
using UnityEngine.AI;

namespace Logic
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Npc : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform destination;

        private void OnValidate()
        {
            if (agent == null)
                agent = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {

            if (destination != null)
                agent.destination = destination.position;
        }
    }
}
