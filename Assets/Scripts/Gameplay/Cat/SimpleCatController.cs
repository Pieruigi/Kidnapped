using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class SimpleCatController : MonoBehaviour
    {
        [System.Serializable]
        class AudioClipData
        {
            [SerializeField]
            public AudioClip clip;

            [SerializeField]
            public float volume = 1;

            [SerializeField]
            public float pitch = 1;
        }

        [SerializeField]
        AudioClipData[] clips;


        Rigidbody rb;
        Animator animator;

        const float walkSpeedDefault = 2;
        const float runSpeedDefault = 3.5f;
        const float trotSpeedDefault = 2.5f;
        float moveSpeed = 3;
        bool moving = false;

        AudioSource audioSource;        

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponentInChildren<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if (moving)
            {
                rb.velocity = rb.transform.forward * moveSpeed;
            }
        }

        private void OnEnable()
        {
            rb.isKinematic = false;
        }

        private void OnDisable()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        public void Walk(float speed = walkSpeedDefault)
        {
            moveSpeed = speed;
            moving = true;
            // Adjust animator speed
            animator.SetFloat("Speed", moveSpeed * 1.5f);
            // Play animation
            animator.SetTrigger("Walk");
        }

        public void Run(float speed = runSpeedDefault)
        {
            moveSpeed = speed;
            moving = true;
            // Adjust animator speed
            animator.SetFloat("Speed", moveSpeed * 1.5f);
            // Play animation
            animator.SetTrigger("Run");
        }

        public void Trot(float speed = trotSpeedDefault)
        {
            moveSpeed = speed;
            moving = true;
            // Adjust animator speed
            animator.SetFloat("Speed", moveSpeed * .75f);
            // Play animation
            animator.SetTrigger("Trot");
        }

        public void Idle()
        {
            moving = false;
            animator.SetTrigger("Idle");
        }

        public void Lick()
        {
            moving = false;
            animator.SetTrigger("Lick");
        }

        public void Rotate(float angle, float duration = 1)
        {
            transform.DORotateQuaternion(Quaternion.Euler(0, angle, 0), duration);
        }

        public void PlayMeow(int clipId, bool loop = false, float delay = 0)
        {
            var data = clips[clipId];
            audioSource.volume = data.volume;
            audioSource.clip = data.clip;
            audioSource.loop = loop;
            if(delay > 0)
                audioSource.PlayDelayed(delay);
            else
                audioSource.Play();
        }

        public void StopMeow()
        {
            audioSource.Stop();
        }
    }

}
