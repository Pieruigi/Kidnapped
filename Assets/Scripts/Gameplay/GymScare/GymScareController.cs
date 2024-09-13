using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class GymScareController : MonoBehaviour, ISavable
    {
        [Header("State 0")]
        [SerializeField]
        GameObject ball;

        [SerializeField]
        Collider ballTrigger;

        [SerializeField]
        Transform ballTarget;

        [SerializeField]
        Collider doorTrigger;

        [SerializeField]
        CrazyDoor door;

        [Header("State 10")]
        [SerializeField]
        Transform ballEnd;


        int state = 0;
        
        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if (!string.IsNullOrEmpty(data))
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

        public void SlamTheDoor()
        {
            door.Close();
        }

     

        public void LaunchTheBall()
        {
            // Update state
            state = 10;

            // Launch the ball against the wall
            ballTrigger.enabled = false;

            // Activate the ball rigidbody
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            // Launch the ball
            Vector3 dir = ballTarget.position - ball.transform.position;
            rb.AddForce(dir.normalized * 30, ForceMode.VelocityChange);
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

            if(state > 0)
            {
                ballTrigger.enabled=false;
                Rigidbody rb = ball.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.position = ballEnd.position;

            }    
            
        }
        #endregion
    }

}
