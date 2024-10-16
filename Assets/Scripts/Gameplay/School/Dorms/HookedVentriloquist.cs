using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Kidnapped
{
    public class HookedVentriloquist : MonoBehaviour
    {
        float minRotSpeed = 20f;
        float maxRotSpeed = 30f;

        float rotSpeed;

        Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //transform.rotation = Quaternion.AngleAxis(rotSpeed * Time.deltaTime, Vector3.up);
            transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
        }

        private void OnEnable()
        {
            // Set random rotation speed
            rotSpeed = Random.Range(minRotSpeed, maxRotSpeed);

            // Set animator starting point
            animator.SetTrigger("Hooked");
            animator.playbackTime = Random.Range(0f, 1f);
        }
    }

}
