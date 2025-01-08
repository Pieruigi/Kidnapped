using EvolveGames;
using Kidnapped.SaveSystem;
using Kidnapped.UI;
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
        PlayerWalkInTwoWayTrigger doorStateTrigger;

        [SerializeField]
        GameObject doorUnblock;

        [SerializeField]
        GameObject doorBlock;

        //[SerializeField]
        //LightActivator externalHintLight;

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
        GameObject[] scaryGroups;

        [SerializeField]
        Light[] scaryCandles;

        [SerializeField]
        GameObject scaryGroupBallPrefab;

        [SerializeField]
        GameObject lilithPrefab;

        [SerializeField]
        int lilithPool = 6;

        [SerializeField]
        InTheFog inTheFogController;

        [SerializeField]
        PlayerWalkInTrigger dialogTrigger;

        [SerializeField]
        DialogController dialogController;

        [SerializeField]
        GameObject mainEntranceBlock;

        [SerializeField]
        GameObject parkTeleportGroup;

        //[SerializeField]
        //VentriloquistJumpScareGroundFloor ventriloquistJumpScare;

        Vector3 scaryBallRotSpeedMin = Vector3.one * 10;
        Vector3 scaryBallRotSpeedMax = Vector3.one * 20;

        int scaryIndex = 0;
#if UNITY_EDITOR
        int state = 0;
#else
        int state = 0;
#endif
        GameObject brokenMannequin;

        List<GameObject> liliths = new List<GameObject>();
        GameObject scaryGroupBall;

        
        private void Awake()
        {
            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = state.ToString();
                
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

        private void OnEnable()
        {
            ballTrigger.GetComponent<PlayerWalkInTrigger>().OnEnter += HandleOnBallTriggerEnter;
            doorTrigger.GetComponent<PlayerWalkInTrigger>().OnEnter += HandleOnDoorTriggerEnter;
            doorStateTrigger.OnExit += HandleOnDoorStateTriggerExit;
            dialogTrigger.OnEnter += HandleOnDialogTriggerEnter;
        }

        private void OnDisable()
        {
            ballTrigger.GetComponent<PlayerWalkInTrigger>().OnEnter -= HandleOnBallTriggerEnter;
            doorTrigger.GetComponent<PlayerWalkInTrigger>().OnEnter -= HandleOnDoorTriggerEnter;
            doorStateTrigger.OnExit -= HandleOnDoorStateTriggerExit;
            dialogTrigger.OnEnter -= HandleOnDialogTriggerEnter;
        }

        private void HandleOnDialogTriggerEnter(PlayerWalkInTrigger arg0)
        {
            // Disable trigger
            dialogTrigger.gameObject.SetActive(false);

            // Play dialog
            dialogController.Play();
        }

        private void HandleOnDoorStateTriggerExit(bool fromBehind)
        {
            if (state > 0)
                return;

            if (fromBehind)
                door.Close();
            else
                door.Open();

        }

        private void HandleOnDoorTriggerEnter(PlayerWalkInTrigger trigger)
        {
            trigger.gameObject.SetActive(false);

            //externalHintLight.SetEnabled(false);

            SlamTheDoor();
        }

        private void HandleOnBallTriggerEnter(PlayerWalkInTrigger trigger)
        {
           
#pragma warning disable CS4014
            LaunchTheBall();
#pragma warning restore CS4014
        }

        void SetDoorBlock()
        {
            doorBlock.GetComponent<ISavable>().Init(true.ToString());
            doorUnblock.GetComponent<ISavable>().Init(false.ToString());
            //doorBlock.SetActive(true);
            //doorUnblock.SetActive(false);
            
        }

        void ResetDoorBlock()
        {
            doorBlock.GetComponent<ISavable>().Init(false.ToString());
            doorUnblock.GetComponent<ISavable>().Init(true.ToString());
            //doorBlock.SetActive(false);
            //doorUnblock.SetActive(true);
        }

        public void SlamTheDoor()
        {
            door.Close();

            if (state > 0)
                return;

            SetDoorBlock();

            ballTrigger.enabled = true;

            // Activate the black cat
            CatController.Instance.StandAndPlayRandom(catTarget.position, catTarget.rotation);
        }

     

        public async Task LaunchTheBall()
        {
            // Update state
            state = 100;

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

            // Play ball sound
            PlayBallBounceAudio();

            await Task.Delay(500);

            CatController.Instance.ScaredAndRunAway(catDestination.position);

            // Disable ventriloquist jump scare
            //ventriloquistJumpScare.SetNotReady();
           
            // Just wait for a while and then flicker the flashlight out and start the new section
            await Task.Delay(2500);

     
            // Flashlight
            Flashlight.Instance.GetComponent<FlashlightFlickerController>().FlickerToDarkeness(HandleOnLightOffBall, HandleOnLightCompleteBall);


            
        }

        async void PlayBallBounceAudio()
        {
            AudioSource bas = ball.GetComponentInChildren<AudioSource>();
            await Task.Delay(350);
            bas.Play();
            await Task.Delay(800);
            bas.Play();
        }

        void HandleOnLightOffBall(float duration)
        {
            // Play new ambience
            GameSceneAudioManager.Instance.PlayAmbience(1);

            // Switch to modern school
            gyms[0].GetComponent<SimpleActivator>().Init(false.ToString());
            gyms[1].GetComponent<SimpleActivator>().Init(true.ToString());

            // Init scary index 
            scaryIndex = 0;

            // Spawn Lilith pool
            SpawnLilithPool();

            // Spawn ball
            scaryGroupBall = Instantiate(scaryGroupBallPrefab);

            scaryGroupBall.AddComponent<RandomRotator>().SetSpeedMinAndMax(scaryBallRotSpeedMin, scaryBallRotSpeedMax);

            // Activate the first scary group
            ActivateScaryGroup(scaryIndex);

            ShowHints();

            //PlayerController.Instance.CanRunning = true;
        }

        async void ShowHints()
        {
            await Task.Delay(1000);
            GameplayHintUI.Instance.ShowHint(1); // Stealth and sprint
        }

        void SpawnLilithPool()
        {
            // Spawn liliths
            for (int i = 0; i < lilithPool; i++)
            {
                var l = Instantiate(lilithPrefab);
                l.SetActive(false);
                l.GetComponent<EvilMaterialSetter>().SetEvil();
                liliths.Add(l);
            }
        }

        void HandleOnLightCompleteBall()
        {
            state = 100;

            // Save state
            SaveManager.Instance.SaveGame();
        }

        async void HandleOnInteraction(ObjectInteractor interactor)
        {
            // Stop liliths
            foreach(var l in liliths)
                l.GetComponent<ScaryGirlMannequin>().SetAgonyState();
            
            // Enable ball physics
            Rigidbody rb = scaryGroupBall.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            // Compute force direction
            Vector3 hDir = Vector3.ProjectOnPlane(rb.position - PlayerController.Instance.transform.position, Vector3.up);
            rb.AddForce(hDir.normalized * 4f + Vector3.up * 5.5f, ForceMode.VelocityChange);
           
            await Task.Delay(3000);

            Utility.SwitchLightOn(scaryCandles[scaryIndex], false);

            scaryGroups[scaryIndex].GetComponentInChildren<ObjectInteractor>().OnInteraction -= HandleOnInteraction;

            // Flashlight
            Flashlight.Instance.GetComponent<FlashlightFlickerController>().FlickerToDarkeness(HandleOnLightOff, HandleOnLightComplete);
        }

        void ActivateScaryGroup(int index)
        {
            // Activate the next scary group
            scaryGroups[index].SetActive(true);

            // Register interaction callback
            var interactor = scaryGroups[index].GetComponentInChildren<ObjectInteractor>();
            interactor.OnInteraction += HandleOnInteraction;

            // Disable the ball rigidbody
            Rigidbody brb = scaryGroupBall.GetComponent<Rigidbody>();
            brb.isKinematic = true;
            brb.velocity = Vector3.zero;
            brb.angularVelocity = Vector3.zero;

            // Move the ball in the interactor
            scaryGroupBall.transform.position = interactor.transform.position;

            // Get the spawn point container
            Transform spawnPoints = scaryGroups[index].transform.Find("SpawnPoints");

            // Deactivate Lilith all
            foreach (var l in liliths)
                l.SetActive(false);
            
            // Activate needed liliths
            for(int i=0; i<spawnPoints.childCount;i++)
            {
                // Set lilith position
                liliths[i].transform.position = spawnPoints.GetChild(i).position;
                liliths[i].transform.rotation = spawnPoints.GetChild(i).rotation;

                // Activate lilith
                liliths[i].SetActive(true);

                // Activate logic
                liliths[i].GetComponent<ScaryGirlMannequin>().Reset();

            }
            
        }

        

        private void HandleOnLightOff(float duration)
        {
            // Deactivate the current scary group
            scaryGroups[scaryIndex].SetActive(false);

            // Update the scary index
            scaryIndex++;

            // Any other group?
            if(scaryIndex < scaryGroups.Length)
            {
                ActivateScaryGroup(scaryIndex);

            }
            else // Completed
            {
                // Reset ambience sound
                GameSceneAudioManager.Instance.PlayAmbience(0);

                // Unspawn liliths
                foreach(var l in liliths)
                    Destroy(l);
                // Clear list
                liliths.Clear();

                // Remove the scary ball
                Destroy(scaryGroupBall);

                // Switch to abandoned school
                gyms[0].GetComponent<SimpleActivator>().Init(true.ToString());
                gyms[1].GetComponent<SimpleActivator>().Init(false.ToString());

                // Update state 
                state = 200;

                // Reset blocks
                //ResetDoorBlock(); // TODO: remove reset blocks and open the internal door
                
            }

            
        }

        

        private void HandleOnLightComplete()
        {
#if DEMO
            //DemoUI.Instance.Show();
            //return;
#endif

            if (state == 200)
            {
                //PlayerController.Instance.CanRunning = false;

                // Set the door open
                door.Open();

                // Block the main entrance of the school
                mainEntranceBlock.SetActive(true);

                // Set InTheFog gameplay ready
                inTheFogController.SetReady();

                // Disable park teleport
                parkTeleportGroup.SetActive(false);

                // Enable dialog trigger 
                dialogTrigger.gameObject.SetActive(true);
                // Save
                SaveManager.Instance.SaveGame();
            }
        }

        void DisableScaryGroupAll()
        {
            foreach (var group in scaryGroups)
            {
                group.SetActive(false);
            }
        }

        public void SetPatchBlockState()
        {
            Init(250.ToString());
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

            //ResetDoorBlock();
            ballTrigger.enabled = false;
            catDeactivator.SetActive(false);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            DisableScaryGroupAll();
            
            // Original gym settings
            if (state == 100) // Step to save ( the cat freaking out )
            {
                doorTrigger.gameObject.SetActive(false);

                rb.isKinematic = false;
                rb.position = ballEnd.position;
                // Init scary index
                scaryIndex = 0;
                // Spawn Lilith pool
                SpawnLilithPool();
                // Spawn ball
                scaryGroupBall = Instantiate(scaryGroupBallPrefab);

                scaryGroupBall.AddComponent<RandomRotator>().SetSpeedMinAndMax(scaryBallRotSpeedMin, scaryBallRotSpeedMax);

                // Activate the first scary group
                ActivateScaryGroup(scaryIndex);

                ShowHints();

                //PlayerController.Instance.CanRunning = true;
            }
            
            // Old gym
            if(state == 200) // Dummy target touched
            {
                rb.isKinematic = false;
                rb.position = ballEnd.position;
                doorTrigger.gameObject.SetActive(false);
                SetDoorBlock();
            }
            else
            {
                // This state is kind of a patch to remove blocks from the external entrance (used in the third part)
                if(state == 250) 
                {
                    rb.isKinematic = false;
                    rb.position = ballEnd.position;
                    doorTrigger.gameObject.SetActive(false);
                    ResetDoorBlock();
                }
            }
        }
        #endregion
    }

}
