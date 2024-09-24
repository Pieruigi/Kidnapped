using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        List<BellController> solution;

        int next = 0;
        int step = 0;

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

            step++;

            if (next == solution.Count)
            {
                OnSolved?.Invoke();
            }
            else
            {
                if (step > 6)
                {
                    OnFailed?.Invoke();
                    step = 0;
                }
            }
        }

  

   
    }

}
