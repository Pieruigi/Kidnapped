using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class BellPuzzleController : MonoBehaviour
    {
        public UnityAction OnSolved;
        public UnityAction OnFailed;

        [SerializeField]
        ObjectInteractor[] solution;

        int next = 0;
        int step = 0;

        //private void OnEnable()
        //{
        //    for(int i = 0; i < solution.Length; i++)
        //    {
        //        solution[i].OnInteraction += () => { Interaction(i); };
        //    }
            
        //}

        //private void OnDisable()
        //{
        //    for (int i = 0; i < solution.Length; i++)
        //    {
        //        solution[i].OnInteraction += () => { Interaction(i); };
        //    }
        //}

        void Interaction(int index)
        {
            Debug.Log($"Index:{index}");

            if (index == next)
                next++;
            else
                next = 0;
            
            step++; 

            if(next == solution.Length)
            {
                OnSolved?.Invoke();
            }
            else
            {
                if(step > 6)
                {
                    OnFailed?.Invoke();
                    step = 0;
                }
            }
        }

   
    }

}
