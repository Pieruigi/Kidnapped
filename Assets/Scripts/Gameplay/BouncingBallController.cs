using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class BouncingBallController : MonoBehaviour
    {
        public UnityAction<int> OnStepCompleted;

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

            //[SerializeField]
            //public GameObject movingTrigger;

            [SerializeField]
            public bool impulse;

            [SerializeField]
            public float moveDelay;

            [SerializeField]
            public float moveLifeTime;

            [SerializeField]
            public bool resetVelocity = false;

        }

        [SerializeField]
        BouncingBall ball;

        [SerializeField]
        List<StepData> steps;

        
        int step = -1;
        public int Step {  get { return step; }  }
        bool moving = false;

        private void Awake()
        {
            //foreach (var step in steps)
            //{
            //    if (step.movingTrigger)
            //    {
            //        step.movingTrigger.GetComponent<BouncingBallMovingTrigger>().SetController(this);
            //        step.movingTrigger.SetActive(false);
                    
            //    }
                    
            //}
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

            //if (steps[step].movingTrigger)
            //    steps[step].movingTrigger.SetActive(false);


            if (steps[step].moveDelay > 0)
                await Task.Delay(System.TimeSpan.FromSeconds(steps[step].moveDelay));
            // Move the ball 
            ball.MoveToDestination(steps[step].destination.position, steps[step].forceMagnitude, steps[step].impulse, steps[step].resetVelocity);
            // Add some delay
            if (steps[step].moveLifeTime > 0)
            {
                await Task.Delay(System.TimeSpan.FromSeconds(steps[step].moveLifeTime));
                // Deactivate the ball
                ball.Deactivate();
            }
           
            OnStepCompleted?.Invoke(step);

            //// Check if there is something ( normally a trigger ) to activate on complete
            //if (steps[step].activateOnComplete)
            //    steps[step].activateOnComplete.SetActive(true);

            // Add a little more delay ( we can also use delay with activation trigger )
            
            //await Task.Delay(System.TimeSpan.FromSeconds(steps[step].nextStepDelay));
            //// Move directly to the next step or use the activation trigger if any
            //if (!steps[step].nextStepActivationTrigger)
            //    MoveToNextStep();
            //else
            //    steps[step].nextStepActivationTrigger.SetActive(true);
        }

        public void ForceStopMoving()
        {
            moving = false;
            ball.Deactivate();
            //OnStepCompleted?.Invoke(step);
        }

        public void MoveToNextStep()
        {
            if (step > steps.Count - 1) // Last step
                return;

            // Update step
            step++;

            // Reset moving flag
            moving = false;
            // Get current step data
            StepData data = steps[step];
            // Deactivate the ball to eventually reset it
            ball.Deactivate();
            // Activate the ball
            ball.Activate(data.target, steps[step].bounceMagnitude);
            

        }

        
    }

}
