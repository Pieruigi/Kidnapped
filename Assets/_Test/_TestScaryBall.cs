using Kidnapped;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestScaryBall : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    Transform destination;

    [SerializeField]
    BouncingBall ball;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) 
        {
            if (ball.IsActive)
                ball.Deactivate();
            else
                ball.Activate(target);
                        
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            if (ball.IsActive)
                ball.MoveToDestination(destination.position, 10, true);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (ball.IsActive)
                ball.MoveToDestination(destination.position, 4, false);
        }
    }
}
