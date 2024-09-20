using Kidnapped;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class GymScareDoorTrigger : MonoBehaviour
    {
        [SerializeField]
        GymScareController controller;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            controller.SlamTheDoor();
        }
    }

}
