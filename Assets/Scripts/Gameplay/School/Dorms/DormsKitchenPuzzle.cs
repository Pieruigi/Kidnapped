using EvolveGames;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class DormsKitchenPuzzle : MonoBehaviour
    {
        public UnityAction OnPuzzleSolved;

        [SerializeField]
        GameObject symbolGroupPrefab;

        [SerializeField]
        GameObject interactorPrefab;

        [SerializeField]
        GameObject ventriloquistPrefab;

        GameObject mannequinGroup;
        Light kitchenLight;
        GameObject symbolGroup;

        List<GameObject> symbols = new List<GameObject>();
        List<GameObject> mannequins = new List<GameObject>();
        List<ObjectInteractor> interactors = new List<ObjectInteractor>();

        int step = 0;
        int[] solution = new int[] { 3, 1, 2, 4 };
        List<int> current = new List<int>();

        List<string[]> steps = new List<string[]>();

        GameObject ventriloquist;
        ObjectInteractor lastInteractor;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("AAAAAAAAAAAAAAAAAAAAA");
                ventriloquist = Instantiate(ventriloquistPrefab);
                // Set rotation 
                //ventriloquist.transform.forward = -Camera.main.transform.forward;
                //// Set parent
                //ventriloquist.transform.parent = Camera.main.transform;
                //// Adjust position
                //ventriloquist.transform.localPosition = new Vector3(0, -0.907f, 0.365f);
                lastInteractor = interactors[0];
                ventriloquist.transform.forward = lastInteractor.transform.forward;
                ventriloquist.transform.position = lastInteractor.transform.position;
                // Set animation
                ventriloquist.GetComponentInChildren<Animator>().SetTrigger("HugToMannequin");
                // Set eyes
                ventriloquist.GetComponent<VentriloquistEyes>().UseScriptedEyes = true;
                
                
            }
#endif
        }

        private void OnEnable()
        {
            if (Flashlight.Instance)
            {
                Flashlight.Instance.OnSwitchedOn += HandleOnFlashlightSwitchedOnOff;
                Flashlight.Instance.OnSwitchedOff += HandleOnFlashlightSwitchedOnOff;
            }


            

        }

        

        private void OnDisable()
        {
            if (Flashlight.Instance)
            {
                Flashlight.Instance.OnSwitchedOn -= HandleOnFlashlightSwitchedOnOff;
                Flashlight.Instance.OnSwitchedOff += HandleOnFlashlightSwitchedOnOff;
            }
        }

        private void HandleOnFlashlightSwitchedOnOff()
        {
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            UpdateInteractionSymbols();
            UpdateWallSymbols();
        }

        void InitSteps()
        {
            steps.Add(new string[] { "S", "W", "R", "P", "X" });
            steps.Add(new string[] { "O", "U", "Z", "B", "Y" });
            steps.Add(new string[] { "I", "F", "C", "L", "V" });
            steps.Add(new string[] { "T", "G", "Q", "J", "K" });
        }

        private async void HandleOnSymbolInteraction(ObjectInteractor interactor)
        {
            // Store the last symbol we interacted with
            lastInteractor = interactor;
            // Disable all interactors
            EnableInteractorAll(false);
            // Get interactor index
            int index = interactors.FindIndex(i => i == interactor);
            // Set current choice
            current.Add(index);

            if (step == steps.Count - 1)
            {
                // Last step, check for solution
                if (IsSolved())
                {
                    // Do somethinkg
                    Debug.Log("Puzzle solved");

                    await Task.Delay(1000);

                    // Flicker
                    FlashlightFlickerController.Instance.FlickerToDarkeness(OnPuzzleSolvedFlicker);

                }
                else
                {
                    Debug.Log("Puzzle failed");
                    current.Clear();
                    step = 0;

                    // Flicker
                    FlashlightFlickerController.Instance.FlickerOnce(OnPuzzleFailedFlicker);
                    
                }
            }
            else
            {
                // Move to the next step
                step++;
                // Switch off the kitchen light
                //kitchenLight.enabled = false;
                Utility.SwitchLightOn(kitchenLight, false);
                // Flicker
                FlashlightFlickerController.Instance.FlickerOnce(OnInteractionFlicker, OnInteractionFlickerCompleted);
            }
        }

        private void OnPuzzleSolvedFlicker(float arg0)
        {
            // Destroy symbols
            Destroy(symbolGroup);
            // Destroy interactors
            foreach(var interactor in interactors)
            {
                Destroy(interactor);
            }
            // Clear the list
            interactors.Clear();

            // Report the parent controller the puzzle has been solved
            OnPuzzleSolved?.Invoke();
        }

        private async void OnPuzzleFailedFlicker()
        {
            // Switch the kitchen light off
            Utility.SwitchLightOn(kitchenLight, false);

            // Instantiate the ventriloquist
            ventriloquist = Instantiate(ventriloquistPrefab);
            ventriloquist.transform.forward = lastInteractor.transform.forward;
            ventriloquist.transform.position = lastInteractor.transform.position;
            // Set animation
            ventriloquist.GetComponentInChildren<Animator>().SetTrigger("HugToMannequin");
            // Set eyes
            ventriloquist.GetComponent<VentriloquistEyes>().UseScriptedEyes = true;

            // Set animation
            Animator animator = ventriloquist.GetComponentInChildren<Animator>();
            animator.SetTrigger("HugToMannequin");

            // Wait a little more
            await Task.Delay(1500);


            // Flicker
            FlashlightFlickerController.Instance.FlickerOnce(OnPuzzleResetFlicker);

            
        }

        private void OnPuzzleResetFlicker()
        {
            // Destroy ventriloquist
            Destroy(ventriloquist);

            // Enable interactors 
            EnableInteractorAll(true);

            // Reset symbols
            UpdateWallSymbols();

            // Reset interactors
            UpdateInteractionSymbols();

            // Enable kitchen light
            Utility.SwitchLightOn(kitchenLight, true);
        }

        private void OnInteractionFlicker()
        {
            // Get the current symbol list
            var s = steps[step];
            // Replace all symbols
            UpdateInteractionSymbols();
            UpdateWallSymbols();
            // Enable interactors
            EnableInteractorAll(true);
        }

        private void OnInteractionFlickerCompleted()
        {
            // Switch on kitchen light
            //kitchenLight.enabled = true;
            Utility.SwitchLightOn(kitchenLight, true);
        }

        void EnableInteractorAll(bool value)
        {
            foreach(var interactor in interactors)
            {
                interactor.gameObject.SetActive(value);
            }
        }

        bool IsSolved()
        {
            if (current.Count < solution.Length)
                return false;

            for(int i=0; i<solution.Length; i++)
            {
                if (current[i] != solution[i])
                    return false;
            }

            return true;
        }

        

        void UpdateWallSymbols()
        {
            for(int i=0; i<symbols.Count;i++)
            {
                if (i == step && !Flashlight.Instance.IsOn)
                    symbols[i].gameObject.SetActive(true);
                else
                    symbols[i].gameObject.SetActive(false);
            }
        }

        void UpdateInteractionSymbols()
        {
            for (int i = 0; i < interactors.Count; i++)
            {
                var interactor = interactors[i];
                interactor.GetComponentInChildren<TMP_Text>().text = steps[step][i];

                if(Flashlight.Instance.IsOn)
                    interactor.gameObject.SetActive(true);
                else
                    interactor.gameObject.SetActive(false);

            }
        }

        public void Init(GameObject mannequinGroup, Light kitchenLight)
        {
            // Set initial step
            step = 0;
            // Reset current
            current.Clear();
            // Init steps
            InitSteps();
            // Set mannequin group
            this.mannequinGroup = mannequinGroup;
            // Read all mannequins in the group skippin the female one
            for(int i=1; i<mannequinGroup.transform.childCount; i++)
            {
                // Get the current mannequin
                var mannequin = mannequinGroup.transform.GetChild(i).gameObject;
                // Add mannequin to the list
                mannequins.Add(mannequinGroup.transform.GetChild(i).gameObject);
                // Create a new symbol interactor
                var interactor = Instantiate(interactorPrefab);
                // Set the position
                interactor.transform.position = mannequin.transform.position;
                interactor.transform.rotation = mannequin.transform.rotation;
                // Add the interactor to the list
                interactors.Add(interactor.GetComponent<ObjectInteractor>());
                // Register callbacks
                interactor.GetComponent<ObjectInteractor>().OnInteraction += HandleOnSymbolInteraction;

            }
            // Set kitchen light
            this.kitchenLight = kitchenLight;
            // Spawn symbol group
            symbolGroup = Instantiate(symbolGroupPrefab);
            // Read all symbols
            for(int i=0; i<symbolGroup.transform.childCount; i++)
            {
                var symbol = symbolGroup.transform.GetChild(i).gameObject;
                // Add new symbol
                symbols.Add(symbol);
                
            }

            // Update wall symbols
            UpdateWallSymbols();

            UpdateInteractionSymbols();
        }

        
    }

}
