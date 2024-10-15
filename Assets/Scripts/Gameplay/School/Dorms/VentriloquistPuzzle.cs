using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class VentriloquistPuzzle : MonoBehaviour
    {
        public UnityAction OnPuzzleSolved;

        //[SerializeField]
        //BoyDorms boyDorms;

        [SerializeField]
        GameObject poseGroupPrefab;

        GameObject poseGroup;

        bool ready = false;

        // Solution is 0, 1, 2, 3, 4 and 5 ( we can't modify the last two mannequins )
        int[] current = new int[] {1, 3, 0, 2, 4, 5};

        Animator[] animators;
        List<ObjectInteractor> interactors;

        string poseParamName = "Pose";
        string typeParamName = "Type";
        string sitDownParamName = "SitDown";

        int animCount = 4;
     
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Z))
            {
                
                StartPuzzle();
            }
#endif
        }

        public void StartPuzzle()
        {
            // Spawn doll group
            poseGroup = Instantiate(poseGroupPrefab, transform.parent);
            poseGroup.transform.localPosition = Vector3.zero;
            poseGroup.transform.localRotation = Quaternion.identity;

            // Fill the animator array
            animators = poseGroup.GetComponentsInChildren<Animator>();

            // Start poses
            for(int i=0; i<animators.Length; i++)
            {
                animators[i].SetInteger(typeParamName, current[i]+1); // Type = value + 1
                animators[i].SetTrigger(poseParamName);
            }

            // Fill the interactor array
            interactors = poseGroup.GetComponentsInChildren<ObjectInteractor>().ToList();
            // Set callbacks
            foreach (ObjectInteractor interactor in interactors)
                interactor.OnInteraction += HandleOnInteraction;
        }

        public void StopPuzzle()
        {
            //// Unregister callbacks
            //foreach (ObjectInteractor interactor in interactors)
            //    interactor.OnInteraction -= HandleOnInteraction;

            // Destroy dolls
            Destroy(poseGroup);
        }

        private async void HandleOnInteraction(ObjectInteractor interactor)
        {
            // Get interactor index
            int index = interactors.FindIndex(i => i == interactor);
            Debug.Log($"Clicked on doll {index}");

            // Get the current value
            int currV = current[index];

            // Update value
            currV++;
            if(currV >= animCount)
                currV = 0;
            current[index] = currV;

            // Get animator
            Animator animator = animators[index];

            // Switch animation
            animator.SetInteger(typeParamName, currV+1);
            animator.SetTrigger(poseParamName);

            // Check for puzzle solution
            if (IsSolved())
            {
                Debug.Log("Puzzle solved");
                // Disable all interactors
                foreach(var i in interactors)
                    i.enabled = false;

                // Add some delay
                await Task.Delay(1000);

                // Sit down
                for (int i = 0; i < animCount; i++)
                    animators[i].SetTrigger(sitDownParamName);

                OnPuzzleSolved?.Invoke();
            }
        }

        bool IsSolved()
        {
            for(int i=0; i<current.Length; i++)
            {
                if (current[i] != i)
                    return false;
            }

            return true;
        }
    }

}
