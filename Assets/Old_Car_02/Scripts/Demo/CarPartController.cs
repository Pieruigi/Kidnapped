using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarPreviewPackage
{
    public class CarPartController : MonoBehaviour
    {
        [SerializeField] private CarParts carPart;
        [SerializeField] private CarManager carManager;

        public void Interact()
        {
            carManager.AnimateDynamicCarPart(carPart);
        }
    }
}
