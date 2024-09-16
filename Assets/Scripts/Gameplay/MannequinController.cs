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
        GameObject killerObject;

        bool canMove = false;
        NavMeshAgent agent;
        float currentRange;


        Vector3 lastTargetPosition;

        DateTime lastCheckTime;
        float checkTime = .1f;
        CharacterController playerCC;




        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            killerObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            lastTargetPosition = PlayerController.Instance.transform.position;
            playerCC = PlayerController.Instance.GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            if ((DateTime.Now - lastCheckTime).TotalSeconds > checkTime)
            {
                currentRange = GetSpottedRange();
                canMove = CheckForMovement();
                lastTargetPosition = PlayerController.Instance.transform.position;
            }

            


            if (!canMove)
            {
                if (agent.hasPath)
                    agent.ResetPath();
            }
            else
            {
                agent.SetDestination(PlayerController.Instance.transform.position);
            }

            if (Vector3.ProjectOnPlane(PlayerController.Instance.transform.position - transform.position, Vector3.up).magnitude < killDistance)
                KillThePlayer();

            //agent.SetDestination(target.position);
        }

        bool CheckForMovement()
        {
            if (playerCC.velocity.magnitude == 0)
                return false;

            if(PlayerController.Instance.IsDying) return false;

            if (Vector3.Distance(PlayerController.Instance.transform.position, transform.position) > currentRange)
                return false;

            if (IsObjectInFrustum(gameObject))
                return false;

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
            Flashlight.Instance.GetComponent<LightFlickerOff>().Play(HandleOnLightOff, HandleOnLightOn);
        }

        void HandleOnLightOn()
        {
            // Stop the player from moving and receiving input
            PlayerController.Instance.PlayerInputEnabled = false;

            // Put something in front of the camera to scare the player
            Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
            killerObject.transform.position = position;
            killerObject.transform.LookAt(Camera.main.transform.position);
            killerObject.SetActive(true);

        }

        void HandleOnLightOff()
        {

        }
    }

}
