using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class BouncingBallMoveTrigger : MonoBehaviour
    {
        BouncingBallController controller;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            controller.Move();
        }

        public void SetController(BouncingBallController controller)
        {
            this.controller = controller;
        }
    }

}
