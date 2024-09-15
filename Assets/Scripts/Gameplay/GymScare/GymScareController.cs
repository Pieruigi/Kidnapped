using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Kidnapped
{
    public class GymScareController : MonoBehaviour, ISavable
    {
        [Header("Ball")]
        [SerializeField]
        GameObject ball;

        [SerializeField]
        Collider ballTrigger;

        [SerializeField]
        Transform ballTarget;

        [SerializeField]
        Transform ballEnd;

        [Header("Doors")]
        [SerializeField]
        Collider doorTrigger;

        [SerializeField]
        ScaryDoor door;

        [SerializeField]
        GameObject doorUnblock;

        [SerializeField]
        GameObject doorBlock;

        [Header("Cat")]
        [SerializeField]
        Transform catTarget;

        [SerializeField]
        Transform catDestination;

        [SerializeField]
        GameObject catDeactivator;

        


        int state = 0;
        
        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = "0";
                
            Init(data);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetDoorBlock()
        {
            doorBlock.SetActive(true);
            doorUnblock.SetActive(false);
            
        }

        void ResetDoorBlock()
        {
            doorBlock.SetActive(false);
            doorUnblock.SetActive(true);
        }

        public void SlamTheDoor()
        {
            state = 10;

            door.Close();

            SetDoorBlock();

            ballTrigger.enabled = true;

            // Activate the black cat
            CatController.Instance.StandAndPlayRandom(catTarget.position, catTarget.rotation);
        }

     

        public async Task LaunchTheBall()
        {
            // Update state
            state = 20;

            // Launch the ball against the wall
            ballTrigger.enabled = false;

            // Activate the ball rigidbody
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            // Launch the ball
            Vector3 dir = ballTarget.position - ball.transform.position;
            rb.AddForce(dir.normalized * 15, ForceMode.VelocityChange);

            // Activate the cat deactivation trigger
            catDeactivator.SetActive(true);

            await Task.Delay(500);

            CatController.Instance.ScaredAndRunAway(catDestination.position);


        }

        #region save system
        [Header("Save System")]
        [SerializeField]
        string code;
        public string GetCode()
        {
            return code;
        }

        public string GetData()
        {
            return state.ToString();
        }

        public void Init(string data)
        {
            state = int.Parse(data);

            ResetDoorBlock();
            ballTrigger.enabled = false;
            catDeactivator.SetActive(false);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            

            if (state == 20)
            {
                rb.isKinematic = false;
                rb.position = ballEnd.position;
            }
            //else if(state == 90) // Completed state
            //{
            //    ResetDoorBlock();
            //    ballTrigger.enabled = false;
                
            //}

        }
        #endregion
    }

}
