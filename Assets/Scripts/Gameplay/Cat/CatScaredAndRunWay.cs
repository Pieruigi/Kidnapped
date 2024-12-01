using Kidnapped;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatScaredAndRunWay : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    Rigidbody rb;

    Vector3 destination;
    public Vector3 Destination { get { return destination; } set { destination = value; } }
    float rotSpeed = 1.5f;
    float moveForce = 20f;
    float elapsed = 0;
    float runTime = .75f;
    float rotationTime = .75f;
    float maxSpeed = 6f;

    public bool JumpDisabled { get; set; } = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

   

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;   
        
        if(elapsed > rotationTime)
        {
            // Rotate
            Vector3 dir = Vector3.ProjectOnPlane(destination - transform.position, Vector3.up);
            Vector3 newFwd = Vector3.MoveTowards(transform.forward, dir.normalized, rotSpeed * Time.deltaTime);
            transform.forward = newFwd;
        }
        
    }

    private void FixedUpdate()
    {
        if (elapsed > runTime)
        {
            Vector3 dir = Vector3.ProjectOnPlane(destination - transform.position, Vector3.up);
            
            rb.AddForce(dir.normalized * moveForce, ForceMode.Acceleration);
        }

        // Clamp max speed
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
            

    }

    private void OnEnable()
    {
        if (!CatController.Instance)
            return;

        // Play sound
        CatController.Instance.Scream();

        // Play scared animation
        if(!JumpDisabled)
        {
            animator.SetTrigger("Scared");
            // Jump
            rb.AddForce(Vector3.up * 3, ForceMode.VelocityChange);
            elapsed = 0;
        }
        else // Don't jump
        {
            animator.SetTrigger("Run");
            elapsed = runTime;
        }
    }

    private void OnDisable()
    {
        //rb.isKinematic = true;
    }

    
}
