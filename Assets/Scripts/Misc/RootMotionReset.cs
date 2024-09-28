using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class RootMotionReset : MonoBehaviour
    {
        [SerializeField]
        Transform root;

        private void OnEnable()
        {
            Reset(); 
        }

        private void OnDisable()
        {
            Reset();
        }

        private void Reset()
        {
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
        }
    }

}
