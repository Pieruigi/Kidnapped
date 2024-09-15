using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class CatStandAndPlayRandom : MonoBehaviour
    {

        [SerializeField]
        Animator animator;

        [SerializeField]
        string[] triggers;

        DateTime idleStartTime;
        
        
        bool inIdle = false;
        string idleName = "idle";

        float minIdleTime = 3f;
        float maxIdleTIme = 5f;
        float idleTime;
        bool inTransition = false;
        Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            //enabled = false;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            bool lastInIdle = inIdle;
            inIdle = animator.GetCurrentAnimatorStateInfo(0).IsName(idleName);
            Debug.Log("Idle:" + inIdle);
            Debug.Log("lastIdle:" + lastInIdle);
            //if (!inIdle)
            //{

            //}
            if (inIdle) // Is in idle
            {
                if (!lastInIdle)
                {
                    idleStartTime = DateTime.Now;
                    idleTime = UnityEngine.Random.Range(minIdleTime, maxIdleTIme);
                }
                else
                {
                    if ((DateTime.Now - idleStartTime).TotalSeconds > idleTime && !inTransition)
                    {
                        string trigger = triggers[UnityEngine.Random.Range(0, triggers.Length)];
                        animator.SetTrigger(trigger);
                        inTransition = true;
                    }
                }
                
            }
            else
            {
                inTransition = false;
            }
            
        }

        private void OnEnable()
        {
            //idleStartTime = DateTime.Now;
            animator.Rebind();
            foreach(var trigger in triggers)
            {
                animator.ResetTrigger(trigger);
            }
            inIdle = false;
            inTransition = false;
            //rb.isKinematic = true;
            //idleStartTime = DateTime.Now;
            //idleTime = UnityEngine.Random.Range(minIdleTime, maxIdleTIme);
            //inIdle = true;
        }
        private void OnDisable()
        {
            animator.Rebind();
            foreach (var trigger in triggers)
            {
                animator.ResetTrigger(trigger);
            }
        }


    }

}
