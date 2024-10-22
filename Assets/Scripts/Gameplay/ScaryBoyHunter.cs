using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Kidnapped
{
    public class ScaryBoyHunter : MonoBehaviour
    {
        public UnityAction<ScaryBoyHunter> OnKillingPlayer;

        enum _State { Idle, Patrol, Chase, Kill }

        static List<ScaryBoyHunter> siblings = new List<ScaryBoyHunter>();

        [SerializeField]
        float walkSpeed = 2.5f;

        [SerializeField]
        float runSpeed = 3.5f;

        [SerializeField]
        Animator animator;

        [SerializeField]
        List<Transform> patrolPoints;

        /// <summary>
        /// 0: player not moving
        /// 1: player crouching ( moving )
        /// 2: player walking
        /// 3: player running
        /// </summary>
        [SerializeField]
        List<float> spotRanges;

        [SerializeField]
        float killRange = 1.5f;

        float idleMinTime = 5;
        float idleMaxTime = 8;

        NavMeshAgent agent;
        float idleTime;
        float idleElapsed = 0;
        bool running = false;

        bool logicDisabled = false;

        string speedParamName = "Speed";
        string agonyParamName = "Agony";
        //string screamParamName = "Scream";

        float patrolPointMinDistance = 7f;

        Vector3 lastSpottedPosition;
        
        
        _State state = _State.Idle;



        private void Awake()
        {
            // Add to the sibling list
            siblings.Add(this);

            agent = GetComponent<NavMeshAgent>();
           
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateState();

            UpdateAnimations();
        }

        private void OnDestroy()
        {
            // Remove from the sibling list
            siblings.Remove(this);
        }

        void UpdateState()
        {
            switch (state)
            {
                case _State.Idle:
                    UpdateIdleState();
                    break;
                case _State.Patrol:
                    UpdatePatrolState();       
                    break;
                case _State.Chase:
                    UpdateChaseState();
                    break;
                case _State.Kill:
                    UpdateKillState();
                    break;
            }
        }

        void EnterPatrolState()
        {
            // Get a random destination
            var target = GetRandomPatrolPoint();
            // Set destination
            agent.SetDestination(target.position);
            // Set state
            state = _State.Patrol;
        }

        void UpdatePatrolState()
        {
            // If the player has been spotted move to chasing state
            if(PlayerSpotted())
            {
                EnterChaseState();
                return;
            }

            if (!agent.hasPath)
            {
                EnterIdleState();
            }
        }

        void EnterIdleState()
        {
            // Reset time
            idleElapsed = 0;
            idleTime = Random.Range(idleMinTime, idleMaxTime);
            // Occasionally set the agony animation
            int anim = Random.Range(0, 4);
            if(anim == 0)
                animator.SetBool(agonyParamName, true);
            // Stop running
            SetRunning(false);
            // Change state
            state = _State.Idle;
        }

       
        void UpdateIdleState()
        {
            // If the player has been spotted move to chasing state
            if (PlayerSpotted())
            {
                EnterChaseState();
                return;
            }

            idleElapsed += Time.deltaTime;
            if (idleElapsed < idleTime)
                return;

            EnterPatrolState();
        }

        void EnterChaseState()
        {
            // Stop moving
            agent.ResetPath();

            // Set the new state
            state = _State.Chase;

            
        }

        void UpdateChaseState()
        {
            // Check kill range
            if(PlayerKilled())
            {
                Debug.Log("KillPlayer");
                EnterKillState();
                return;
            }

            // Chase
            if (!PlayerSpotted() && !agent.hasPath)
            {
                EnterIdleState();
                return;
            }

            agent.SetDestination(lastSpottedPosition);
        }

        void EnterKillState()
        {
            state = _State.Kill;

            // Stop moving
            agent.ResetPath();
            agent.enabled = false;

            // If the player is already dying return ( maybe another Puck is already killing the player )
            if (PlayerController.Instance.IsDying)
                return;

            OnKillingPlayer?.Invoke(this);

            
        }

        void UpdateKillState() { }

       

        bool PlayerSpotted()
        {
            // Set the range based on the player movement
            float range = spotRanges[0]; // Default: player not moving at all
            if(PlayerController.Instance.characterController.velocity.magnitude > 0f) // Player is moving
            {
                if (PlayerController.Instance.IsRunning)
                    range = spotRanges[3];
                else if (PlayerController.Instance.IsCrouching)
                    range = spotRanges[1];
                else 
                    range = spotRanges[2]; // Walking
            }            
            
            // Check the player distance
            float distance = Vector3.Distance(PlayerController.Instance.transform.position, transform.position);
            if(distance < range)
            {
                lastSpottedPosition = PlayerController.Instance.transform.position;
                return true; // Spotted
            }

            return false;
        }

        bool PlayerKilled()
        {
            float distance = Vector3.Distance(PlayerController.Instance.transform.position, transform.position);
            if (distance < killRange)
                return true;

            return false;
        }

        void UpdateAnimations()
        {
            if (agent.velocity.magnitude > 0 && animator.GetBool(agonyParamName))
                animator.SetBool(agonyParamName, false);
           
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }

        void SetRunning(bool value)
        {
            running = value;
            if (value)
                agent.speed = runSpeed;
            else
                agent.speed = walkSpeed;  
        }

        Transform GetRandomPatrolPoint()
        {
            // Get all the available patrol points
            List<Transform> candidates = new List<Transform>();
            for(int i=0; i<patrolPoints.Count; i++)
            {
                // Compute distance between agent and patrol point
                float distance = Vector3.Distance(transform.position, patrolPoints[i].position);
                // TODO: also check the others
                if(distance > patrolPointMinDistance) // We exclude the last chosen one if any
                    candidates.Add(patrolPoints[i]);
            }

            // Choose a random destination
            return candidates[Random.Range(0, candidates.Count)];
            
        }

        public void ForceDestination(Vector3 destination, bool run)
        {
            //lastPatrolPoint = null;
            agent.destination = destination;
            SetRunning(run);
            state = _State.Patrol;
        }

        public void DisableLogic()
        {
            logicDisabled = true;
        }

        public void EnableLogic()
        {
            logicDisabled = false;
        }

        public void SetAgonyAnimation()
        {
            state = _State.Idle;
            animator.SetBool(agonyParamName, true);
        }

        public void SetPatrolPoints(List<Transform> patrolPoints)
        {
            this.patrolPoints = patrolPoints;
        }
    }

}
