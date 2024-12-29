using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class AmbienceSwitcher : MonoBehaviour
    {
        [SerializeField]
        int positiveIndex = 0; // Along the forward axis

        [SerializeField]
        int negativeIndex = 4; // Along the backward axis

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(Tags.Player))
                return;

            float dot = Vector3.Dot(Vector3.ProjectOnPlane(other.transform.position - transform.position, Vector3.up), transform.forward);
            int newAmbience = positiveIndex;
            if (dot < 0) 
            {
                newAmbience = negativeIndex;
            }
            GameSceneAudioManager.Instance.FadeInAmbient(newAmbience);
        }
    }

}
