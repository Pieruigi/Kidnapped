using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
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

        [SerializeField]
        GameObject[] gyms;

        [SerializeField]
        ScaryGroup[] scaryGroups;


        int scaryIndex = 0;
        int state = 0;

        GameObject brokenMannequin;
        
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
#if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.K))
            {
                Flashlight.Instance.GetComponent<FlashlightFlickerOff>().Play(HandleOnLightOff, HandleOnLightOn);
            }
#endif

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

            // Just wait for a while and then flicker the flashlight out and start the new section
            await Task.Delay(2500);

            // Flashlight
            Flashlight.Instance.GetComponent<FlashlightFlickerOff>().Play(HandleOnLightOff, HandleOnLightOn);


            
        }

        private void HandleOnLightOff()
        {
            

            // Switch to modern school
            gyms[0].GetComponent<SimpleActivator>().Init(false.ToString());
            gyms[1].GetComponent<SimpleActivator>().Init(true.ToString());

            // Create first scary group
            scaryIndex = 0;
            scaryGroups[scaryIndex].Create();
        }

        

        private void HandleOnLightOn()
        {
            // Set the new state
            state = 30;

            // Save state
            SaveManager.Instance.SaveGame();
        }

        void DisableScaryGroupAll()
        {
            foreach(var group in scaryGroups)
            {
                group.Release();
            }
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
            DisableScaryGroupAll();

            if (state == 30) // Step to save ( the cat freaking out )
            {
                rb.isKinematic = false;
                rb.position = ballEnd.position;
                scaryIndex = 0;
                scaryGroups[scaryIndex].Create();
                     
            }
            

        }
        #endregion
    }

}
