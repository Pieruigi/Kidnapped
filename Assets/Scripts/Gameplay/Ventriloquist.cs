using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class Ventriloquist : MonoBehaviour
    {

        [SerializeField]
        Animator animator;

        [SerializeField]
        float maxAngle;

        [SerializeField]
        AudioSource laughAudioSource;

        //[SerializeField]
        //GameObject leftEye;

        //[SerializeField]
        //GameObject rightEye;

        bool playing = false;

        bool playerInside = false;
        bool playerVisible = false;

        float elapsed = 0;
        float time = baseTime;
        const float baseTime = 3f;
        float randomValue = .2f;
        int side = 0; // -1:left, 0:middle, 1:right

        bool exitToReset = false;

        private void Awake()
        {
            ResetTime();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
         
        }

        //private void LateUpdate()
        //{
        //    if (!playing)
        //        return;

        //    if (playerVisible)
        //    {
        //        leftEye.transform.LookAt(Camera.main.transform.position);
        //        rightEye.transform.LookAt(Camera.main.transform.position);
        //    }
        //    else
        //    {
        //        leftEye.transform.rotation = Quaternion.RotateTowards(leftEye.transform.rotation, Quaternion.identity, Time.deltaTime);
        //        rightEye.transform.rotation = Quaternion.RotateTowards(rightEye.transform.rotation, Quaternion.identity, Time.deltaTime);
        //    }
        //}

        void ResetTime()
        {
            time = Random.Range(baseTime * (1f - 0.2f), baseTime * (1f + .2f));
            elapsed = 0;
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;
            //playerInside = true;    

            if (playing || exitToReset)
                return;

            // Check if the player is visible
            float angle = Vector3.SignedAngle(Vector3.ProjectOnPlane(PlayerController.Instance.transform.position - transform.position, Vector3.up), transform.forward, Vector3.up);
            var playerVisible = (Mathf.Abs(angle) < maxAngle) ? true : false;

            if (!playerVisible)
                return;

            exitToReset = true;
            float sideAngle = maxAngle / 2f;
            // Is player approaching from left or right
            side = (Mathf.Abs(angle) < sideAngle) ? 0 : (angle < 0) ? 1 : -1;
            // Play animation
            playing = true;
            int type = 0;
            bool mirror = false;

            if (side != 0)
            {
                type = 1;
                mirror = (side < 0) ? false : true;
            }
            
            animator.SetInteger("Type", type);
            animator.SetBool("Mirror", mirror);
            animator.SetTrigger("Creepy");
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;
            //playerInside = false;
            //playerVisible = false;
            exitToReset = false;
        }

        /// <summary>
        /// Animation event
        /// </summary>
        public void End()
        {
            playing = false;

        }

        public void Laugh()
        {
            if(laughAudioSource)
                laughAudioSource.Play();
        }
        
    }

}
