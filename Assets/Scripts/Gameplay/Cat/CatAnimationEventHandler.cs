using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class CatAnimationEventHandler : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Meow()
        {
            CatController.Instance.Meow();
        }

    }

}
