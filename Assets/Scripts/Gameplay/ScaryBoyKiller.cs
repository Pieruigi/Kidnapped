using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class ScaryBoyKiller : MonoBehaviour
    {
        [SerializeField]
        float speed = 5f;

        Transform target;

        string runAnimParam = "Run";
        Animator animator;

        private void Awake()
        {
            target = PlayerController.Instance.transform;
            animator = GetComponentInChildren<Animator>();
            animator.SetTrigger(runAnimParam);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            // Look at the player
            Vector3 direction = Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up);
            transform.forward = direction;

            // Run
            transform.position += direction * speed * Time.deltaTime;

        }

        
    }

}
