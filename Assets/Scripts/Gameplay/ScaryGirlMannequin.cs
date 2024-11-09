using EvolveGames;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Kidnapped
{
    public class ScaryGirlMannequin : MonoBehaviour
    {
        [SerializeField]
        float crouchRange = 2f;

        [SerializeField]
        float walkRange = 3f;

        [SerializeField]
        float runRange = 5f;

        [SerializeField]
        float moveSpeed = .2f;

        [SerializeField]
        float killDistance = 2f;

        [SerializeField]
        GameObject[] killingHeads;

        [SerializeField]
        Material evilMaterial;

        [SerializeField]
        Renderer evilRenderer;

        [SerializeField]
        GameObject character;

        bool canMove = false;
        NavMeshAgent agent;
        float currentRange;


        //Vector3 lastTargetPosition;

        DateTime lastCheckTime;
        float checkTime = .1f;
        CharacterController playerCC;

        Animator animator;

        string walkParam = "Walk";
        string typeParam = "Type";
        string speedParam = "Speed";
        string killParam = "Kill";
        string agonyParam = "Agony";

        int walkAnimCount = 2;
        
        bool logic = false;


        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            foreach(var g in killingHeads) 
                g.SetActive(false);
            animator = GetComponentInChildren<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            //lastTargetPosition = PlayerController.Instance.transform.position;
            playerCC = PlayerController.Instance.GetComponent<CharacterController>();
            
            SetRandomWalkAnimation();
        }

        // Update is called once per frame
        void Update()
        {
            if(!logic) return;

            if (PlayerController.Instance.IsDying)
            {
                //agent.ResetPath();
                agent.enabled = false;
                return;
            }
                

            if ((DateTime.Now - lastCheckTime).TotalSeconds > checkTime)
            {
                currentRange = GetSpottedRange();
                canMove = CheckForMovement();
                //lastTargetPosition = PlayerController.Instance.transform.position;
            }

            


            if (!canMove)
            {
                if (agent.hasPath)
                {
                    agent.ResetPath();
                }
            }
            else
            {
                agent.SetDestination(PlayerController.Instance.transform.position);
            }

            if (Vector3.ProjectOnPlane(PlayerController.Instance.transform.position - transform.position, Vector3.up).magnitude < killDistance)
            {
                animator.SetFloat(speedParam, 0);
                KillThePlayer();
            }
            else
            {
                animator.SetFloat(speedParam, agent.velocity.magnitude);
            }
            
        }

       

        bool CheckForMovement()
        {
            if (Vector3.ProjectOnPlane(playerCC.velocity, Vector3.up).magnitude == 0)
                return false;

            if(PlayerController.Instance.IsDying) return false;

            if (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) > currentRange)
                return false;

            //if (IsObjectInFrustum(gameObject))
            //    return false;

            return true;
        }

        bool IsObjectInFrustum(GameObject obj)
        {
            // Get camera frustum planes
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

            // Get the rendering cube
            Renderer renderer = obj.GetComponent<Renderer>();

            // Check bounds
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }

        float GetSpottedRange()
        {
            if (playerCC.velocity.magnitude == 0)
                return 0;

            if (PlayerController.Instance.IsCrouching) return crouchRange;

            if (PlayerController.Instance.IsRunning) return runRange;

            return walkRange;
        }

        void KillThePlayer()
        {
            Debug.Log("Killing the player");

            KillManager.Instance.Kill(KillManager.Killer.Lilith);

        }

        void SetRandomWalkAnimation()
        {
            int type = UnityEngine.Random.Range(0, walkAnimCount);
            animator.SetInteger(typeParam, type);
            if (type == 2) // Crawling
            {
                agent.agentTypeID = NavMesh.GetSettingsByIndex(1).agentTypeID;
            }
            animator.SetTrigger(walkParam);
        }

        public void Reset()
        {
            logic = true;
            agent.enabled = true;
            SetRandomWalkAnimation();
        }


        public void SetAgonyState()
        {
            logic = false;
            agent.enabled = false;
            animator.SetTrigger(agonyParam);

        }

    }

}
