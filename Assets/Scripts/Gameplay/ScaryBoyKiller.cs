using EvolveGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class ScaryBoyKiller : MonoBehaviour
    {
        public UnityAction<ScaryBoyKiller> OnPlayerKilled;

        enum _State { Chasing, Killing }

        [SerializeField]
        float speed = 5f;

        Transform target;

        string runAnimParam = "Run";
        string killAnimParam = "Kill";
        Animator animator;

        float killDistance = 2;

        _State state = _State.Chasing;

        private void Awake()
        {
            target = PlayerController.Instance.transform;
            animator = GetComponentInChildren<Animator>();
            animator.SetTrigger(runAnimParam);
            GetComponent<EvilMaterialSetter>().SetEvil();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            switch(state)
            {
                case _State.Chasing:
                    ChaseThePlayer();
                    break;
                    case _State.Killing:

                    break;
            }
            
            
        }

        void ChaseThePlayer()
        {
            // Look at the player
            Vector3 direction = Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up);
            transform.forward = direction;

            // Run
            transform.position += direction * speed * Time.deltaTime;

            if(direction.magnitude < killDistance)
            {
                EnterKillingState();
            }
        }
        
        void EnterKillingState()
        {
            // Flicker and watch
            FlashlightFlickerController.Instance.FlickerOnce(OnLightOff, OnFlickerCompleted);

            state = _State.Killing;
        }

        private async void OnFlickerCompleted()
        {
            // Add some delay
            await Task.Delay(500);
            // Fade and reload
            GameManager.Instance.FadeOutAndReloadAfterDeath();
        }

        private void OnLightOff()
        {
            // Set pose
            animator.SetTrigger(killAnimParam);
            // Disable input
            PlayerController.Instance.PlayerInputEnabled = false;
            // Attach the killer to the camera
            transform.parent = Camera.main.transform;
            // Look at the player
            transform.forward = -Camera.main.transform.forward;
            // Set local position
            transform.localPosition = new Vector3(-0.043f, -1.364f, 0.692f);
            // Set local rotation
            transform.localEulerAngles = new Vector3(0, -225.7f, 0.034f);
            // Move back
            //transform.position += -transform.forward * 2f;
            // Change fov
            Camera.main.fieldOfView = 40;
            

        }

        void KillThePlayer()
        {

        }
    }

}
