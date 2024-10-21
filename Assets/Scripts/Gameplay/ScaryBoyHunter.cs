using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Kidnapped
{
    public class ScaryBoyHunter : MonoBehaviour
    {

        [SerializeField]
        float walkSpeed = 2.5f;

        [SerializeField]
        float runSpeed = 3.5f;

        [SerializeField]
        Animator animator;

        [SerializeField]
        List<GameObject> patrolPoints;

        float idleMinTime = 5;
        float idleMaxTime = 8;

        NavMeshAgent agent;
        float idleTime;
        bool running = false;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetRunning(bool value)
        {
            running = value;
            if (value)
                agent.speed = runSpeed;
            else
                agent.speed = walkSpeed;  
        }

        public void ForceDestination(Vector3 destination, bool run)
        {
            agent.destination = destination;
            SetRunning(run);
        }

    }

}
