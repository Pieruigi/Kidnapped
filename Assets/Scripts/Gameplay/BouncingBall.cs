using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class BouncingBall : MonoBehaviour
    {

        [SerializeField]
        AudioSource audiSource;

        [SerializeField]
        float bounceMagnitude = 5;

        Rigidbody rb;

        bool isActive = false;
        public bool IsActive
        {
            get { return isActive; }
        }

        Vector3 forceDirection;
        bool applyConstantForce = false;
        float forceMagnitude;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void FixedUpdate()
        {
            if (applyConstantForce)
            {
                rb.AddForce(forceDirection * forceMagnitude, ForceMode.Acceleration);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Add bouncing force
            rb.AddForce(Vector3.up * bounceMagnitude, ForceMode.VelocityChange);
            // Play sound
            audiSource.Play();
        }

        public void Activate(Transform target, float bounceMagnitude = 5)
        {
            if (isActive)
                return;

            Debug.Log($"Activating ball, target:{target}");
            isActive = true;
            rb.isKinematic = true;
            transform.position = target.position;
            rb.isKinematic = false;
            //rb.position = target.position + Vector3.up * 1.3f ;
            rb.velocity = Vector3.zero;
           
            applyConstantForce = false;
            gameObject.SetActive(true);
        }


        public void Deactivate()
        {
            if(!isActive) return;
            isActive = false;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            applyConstantForce = false;
            gameObject.SetActive(false);
        }

        public void MoveToDestination(Vector3 destination, float forceMagnitude, bool impulse)
        {
            applyConstantForce = !impulse;
            this.forceMagnitude = forceMagnitude;
            Vector3 direction = destination - rb.position;
            direction = direction.normalized;
            if (impulse)
                rb.AddForce(direction * forceMagnitude, ForceMode.VelocityChange);
            else
                forceDirection = direction;
           
        }

        public Vector3 GetPosition() { return rb.position; }
    }

}
