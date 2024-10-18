using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Kidnapped
{
    public class DormsKitchenPuzzle : MonoBehaviour
    {

        [SerializeField]
        GameObject symbolGroupPrefab;

        [SerializeField]
        GameObject interactorPrefab;

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

        private void HandleOnSymbolInteraction(ObjectInteractor interactor)
        {
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
                }
                else
                {
                    Debug.Log("Puzzle failed");
                    current.Clear();
                    step = 0;

                    // Flicker
                    //FlashlightFlickerController.Instance.FlickerAndWatch()
                }
            }
            else
            {
                // Move to the next step
                step++;
                // Switch off the kitchen light
                kitchenLight.enabled = false;
                // Flicker
                FlashlightFlickerController.Instance.FlickerOnce(OnInteractionFlicker, OnInteractionFlickerCompleted);
            }
        }

       

        private void OnPuzzleFailedFlicker()
        {
            // 
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
            kitchenLight.enabled = true;
        }

        void EnableInteractorAll(bool value)
        {
            foreach(var interactor in interactors)
            {
                interactor.enabled = value;
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
