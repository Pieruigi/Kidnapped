using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Kidnapped
{
    public class BouncingBallController : MonoBehaviour
    {
        [System.Serializable]
        class StepData
        {
            [SerializeField]
            public Transform target;

            [SerializeField]
            public float bounceMagnitude;

            [SerializeField]
            public Transform destination;

            [SerializeField]
            public float forceMagnitude;

            [SerializeField]
            public float playerDistance = 5;

            [SerializeField]
            public GameObject moveTrigger;

            [SerializeField]
            public bool impulse;

            [SerializeField]
            public float moveDelay;

            [SerializeField]
            public float moveLifeTime;

            [SerializeField]
            public float nextStepDelay;
        }

        [SerializeField]
        BouncingBall ball;

        [SerializeField]
        List<StepData> steps;

        
        int step = -1;
        bool moving = false;

        private void Awake()
        {
            foreach (var step in steps)
            {
                if (step.moveTrigger)
                {
                    step.moveTrigger.GetComponent<BouncingBallMoveTrigger>().SetController(this);
                    step.moveTrigger.SetActive(false);
                }
                    
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (step > steps.Count - 1) // Last step reached
                return;

            if (moving || steps[step].playerDistance < 0) return;

            // Player distance
            float distance = Vector3.Distance(PlayerController.Instance.transform.position, ball.GetPosition());
            if (distance < steps[step].playerDistance)
            {
                Move();
                
            }
        }

        public async void Move()
        {
            moving = true;

            if (steps[step].moveTrigger)
                steps[step].moveTrigger.SetActive(false);


            if (steps[step].moveDelay > 0)
                await Task.Delay(System.TimeSpan.FromSeconds(steps[step].moveDelay));
            // Move the ball 
            ball.MoveToDestination(steps[step].destination.position, steps[step].forceMagnitude, steps[step].impulse);
            // Add some delay
            await Task.Delay(System.TimeSpan.FromSeconds(steps[step].moveLifeTime));
            // Deactivate the ball
            ball.Deactivate();
            // Add a little more of delay
            await Task.Delay(System.TimeSpan.FromSeconds(steps[step].nextStepDelay));
            // Move to next step
            MoveToNextStep();
        }

        public void MoveToNextStep()
        {
            if (step > steps.Count - 1) // Last step
                return;

            step++;


            moving = false;
            Debug.Log($"The bouncing ball step index:{step}");
            StepData data = steps[step];

            if (steps[step].moveTrigger)
                steps[step].moveTrigger.SetActive(true);
            //Debug.Log($"The bouncing ball step:{data}");
            ball.Deactivate();
            
            ball.Activate(data.target, steps[step].bounceMagnitude);
            

        }
    }

}
