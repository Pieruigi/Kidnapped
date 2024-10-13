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
        GameObject leftEye;

        [SerializeField]
        GameObject rightEye;

        bool busy = false;

        bool playerInside = false;
        bool playerVisible = false;

        float elapsed = 0;
        float time = baseTime;
        const float baseTime = 3f;
        float randomValue = .2f;
        int side = 0; // -1:left, 0:middle, 1:right

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
            if (busy || !playerInside) return;

            if (playerInside)
            {
                // Check if the player is infront of the ventriloquist 
                float angle = Vector3.SignedAngle(Vector3.ProjectOnPlane(PlayerController.Instance.transform.position - transform.position, Vector3.up), transform.forward, Vector3.up);
                playerVisible = (Mathf.Abs(angle) < maxAngle) ? true : false;
                float sideAngle = maxAngle / 2f;
                //if(Mathf.Abs(angle) < sideAngle)
                //    side = 0;
                //else
                //    side = sideAngle < 0 ? -1 : 1;
                //if (Mathf.Abs(angle) < sideAngle)
                //{
                //    side = 0;
                //}
                //else
                //{
                //    if(angle < 0)
                //    {

                //    }
                //}
                    side = (Mathf.Abs(angle) < sideAngle) ? 0 : (angle < 0) ? 1 : -1;
            }
       
            if (!playerVisible)
            {
                elapsed = 0; // Reset
            }
            else
            {
                elapsed += Time.deltaTime;
                if(elapsed > time)
                {
                    ResetTime();
                    busy = true;
                    int type = 0;
                    bool mirror = false;

                    if(side != 0)
                    {
                        type = 1;
                        mirror = (side < 0) ? false : true;
                        
                    }

                    animator.SetInteger("Type", type);
                    animator.SetBool("Mirror", mirror);
                    animator.SetTrigger("Creepy");

                    Debug.Log("Creepy");
                }
            }
            
        }

        private void LateUpdate()
        {
            if (!busy)
                return;

            if (playerVisible)
            {
                leftEye.transform.LookAt(Camera.main.transform.position);
                rightEye.transform.LookAt(Camera.main.transform.position);
            }
            else
            {
                leftEye.transform.rotation = Quaternion.RotateTowards(leftEye.transform.rotation, Quaternion.identity, Time.deltaTime);
                rightEye.transform.rotation = Quaternion.RotateTowards(rightEye.transform.rotation, Quaternion.identity, Time.deltaTime);
            }
        }

        void ResetTime()
        {
            time = Random.Range(baseTime * (1f - 0.2f), baseTime * (1f + .2f));
            elapsed = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;
            playerInside = true;    
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;
            playerInside = false;
            playerVisible = false;
        }

        public void End()
        {
            busy = false;

        }

        
    }

}
