using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class PlayerLeftArm : MonoBehaviour
    {
        public Transform target;
        public float speed = 2.5f;
        public Transform positionTarget;
        public static PlayerLeftArm Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void LateUpdate()
        {
            Vector3 dir = target.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed * Time.deltaTime);
            transform.position = positionTarget.position;
        }
    }
}

