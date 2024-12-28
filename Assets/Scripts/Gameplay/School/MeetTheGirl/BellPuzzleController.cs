using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class BellPuzzleController : MonoBehaviour
    {
        public UnityAction OnSolved;
        public UnityAction OnFailed;

        [SerializeField]
        List<BellController> solution;

        int next = 0;
    
        private void OnEnable()
        {
            BellController.OnRing += HandleOnBellRing;

        }

        private void OnDisable()
        {
            BellController.OnRing -= HandleOnBellRing;
        }

        void HandleOnBellRing(BellController bell)
        {
            // Get the bell index
            int index = solution.FindIndex(b => b == bell);

            Debug.Log($"Index:{index}");

            if (index == next)
                next++;
            else
                next = 0;

            if (next == solution.Count)
            {
                OnSolved?.Invoke();
            }
            else
            {
                if(next == 0)
                    OnFailed?.Invoke();
            }
        }

  

   
    }

}
