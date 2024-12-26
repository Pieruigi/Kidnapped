using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class SimpleCatRotationTrigger : MonoBehaviour
    {
        [SerializeField]
        Transform direction;

        [SerializeField]
        float duration;


        private void OnTriggerEnter(Collider other)
        {
            var controller = other.GetComponent<SimpleCatController>();
            if (!controller)
                return;

            controller.Rotate(Quaternion.Angle(controller.transform.rotation, direction.rotation), duration);
        }
    }

}
