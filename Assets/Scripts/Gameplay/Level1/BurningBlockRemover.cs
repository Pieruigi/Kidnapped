using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class BurningBlockRemover : MonoBehaviour
    {
        [SerializeField]
        DoorController doorController;

        [SerializeField]
        GameObject block;

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
            DoorController.OnDoorOpenFailed += HandleOnOpenFailed;
        }

        private void OnDisable()
        {
            DoorController.OnDoorOpenFailed -= HandleOnOpenFailed;
        }

        private void HandleOnOpenFailed(DoorController arg0)
        {
            block.SetActive(false);
            doorController.GetComponentInParent<BurningController>().StartBurning();
        }
    }

}
