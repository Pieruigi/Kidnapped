using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class TeleportTrigger : MonoBehaviour
    {
        [SerializeField]
        Transform target;

        [SerializeField]
        bool fastFlicker = false;

        bool busy = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            busy = true;

            if (fastFlicker)
            {
                FlashlightFlickerController.Instance.FlickerOnce(() =>
                {
                    PlayerController.Instance.ForcePositionAndRotation(target.position, target.rotation);
                    busy = false;
                });
            }
            else
            {
                FlashlightFlickerController.Instance.FlickerToDarkeness((d) =>
                {
                    PlayerController.Instance.ForcePositionAndRotation(target.position, target.rotation);
                    busy = false;
                });
            }
            
        }
    }
}

