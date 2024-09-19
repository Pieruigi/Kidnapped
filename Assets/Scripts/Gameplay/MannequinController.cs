using EvolveGames;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Kidnapped
{
    public class MannequinController : MonoBehaviour
    {
        [System.Serializable]
        private class KillingPoseInfo
        {
            
            [SerializeField]
            public float cameraFov;

            [SerializeField]
            public Vector3 cameraEulers;

            [SerializeField]
            public Vector3 characterPosition;

            [SerializeField]
            public Vector3 characterEulers;
        }
   
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

        [SerializeField]
        KillingPoseInfo[] killingInfos;

        bool canMove = false;
        NavMeshAgent agent;
        float currentRange;


        Vector3 lastTargetPosition;

        DateTime lastCheckTime;
        float checkTime = .1f;
        CharacterController playerCC;

        Animator animator;

        string walkParam = "Walk";
        string typeParam = "Type";
        string speedParam = "Speed";
        string killParam = "Kill";

        int walkAnimCount = 2;
        int killAnimCount = 2;

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
            lastTargetPosition = PlayerController.Instance.transform.position;
            playerCC = PlayerController.Instance.GetComponent<CharacterController>();
            SetRandomWalkAnimation();
        }

        // Update is called once per frame
        void Update()
        {
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
                lastTargetPosition = PlayerController.Instance.transform.position;
            }

            


            if (!canMove)
            {
                if (agent.hasPath)
                {
                    agent.ResetPath();
                    // Change animation
                    //SetRandomWalkAnimation();
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
                

            //agent.SetDestination(target.position);
        }

        void SetRandomWalkAnimation()
        {
            //animator.SetInteger(typeParam, UnityEngine.Random.Range(0, walkAnimCount));
            //animator.SetInteger(typeParam, 2);
            animator.SetTrigger(walkParam);
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
            // Ottieni i piani del frustum della camera
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

            // Ottieni il renderer del cubo per accedere ai bounds
            Renderer renderer = obj.GetComponent<Renderer>();

            // Controlla se i bounds del cubo sono all'interno del frustum
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
           
            // Avoid to get killed by more than one mannequin
            if (PlayerController.Instance.IsDying)
                return;

            PlayerController.Instance.IsDying = true;
            PlayerController.Instance.InteractionDisabled = true;

            // Flicker off the flashlight
            Flashlight.Instance.GetComponent<FlashlightFlickerOff>().Play(HandleOnLightOff, HandleOnLightOn, HandleOnFlickerComplete);
        }

        void HandleOnLightOn()
        {
            
            

        }

        void HandleOnLightOff()
        {
            // Stop the player from moving and receiving input
            PlayerController.Instance.PlayerInputEnabled = false;

            //// Set fov
            //Camera.main.fieldOfView = 25;

            //// Put something in front of the camera to scare the player
            //Vector3 position = Camera.main.transform.position;
            //Quaternion rotation = Camera.main.transform.rotation;

            // Disable agent
            agent.enabled = false;

            // Change to evil material
            evilRenderer.material = evilMaterial;

            // Switch to killing pose
            int type = UnityEngine.Random.Range(0, killAnimCount);
            type = 0;
            animator.SetInteger(typeParam, type);
            animator.SetTrigger(killParam);

            // Get the killing pose info
            KillingPoseInfo info = killingInfos[type];

            // Move the character under the camera
            character.transform.parent = Camera.main.transform;

            // Set position and rotation
            character.transform.localPosition = info.characterPosition;
            character.transform.localEulerAngles = info.characterEulers;

            // Set camera
            Camera.main.fieldOfView = info.cameraFov;
            Camera.main.transform.GetChild(0).GetComponent<Camera>().fieldOfView = info.cameraFov;
            Camera.main.transform.localEulerAngles = info.cameraEulers;

            //// Choose a random killing heads
            //GameObject kh = killingHeads[UnityEngine.Random.Range(0, killingHeads.Length)];
            //kh.transform.parent = Camera.main.transform;
            //kh.transform.localPosition = Vector3.zero;
            //kh.transform.localRotation = Quaternion.identity;
            //kh.SetActive(true);

            //root.SetActive(false);
        }

        void HandleOnFlickerComplete()
        {
            GameManager.Instance.FadeOutAndReloadAfterDeath();
        }
    }

}
