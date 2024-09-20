using Kidnapped;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class CatDeactivator : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(Tags.Cat))
                return;

            CatController.Instance.ResetAll();

            gameObject.SetActive(false);
        }
    }

}
