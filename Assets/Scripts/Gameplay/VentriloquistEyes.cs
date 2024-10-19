using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class VentriloquistEyes : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> animatedEyes;

        [SerializeField]
        List<GameObject> scriptedEyes;

        [SerializeField]
        bool useScriptedEyes = false;
        public bool UseScriptedEyes
        {
            get { return useScriptedEyes; }
            set 
            {
                useScriptedEyes = value;
                foreach (GameObject go in animatedEyes)
                    go.SetActive(!value);
                foreach(GameObject go in scriptedEyes)
                    go.SetActive(value);
                
            }
        }

        [SerializeField]
        Transform target = null;
        public Transform ScriptedEyesTarget
        {
            get { return target; }
            set { target = value; }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            if (!useScriptedEyes)
                return;
            if(!target)
                target = Camera.main.transform;

            // Set eyes forward
            foreach(GameObject go in scriptedEyes)
            {
                Vector3 dir = target.position - go.transform.position;
                go.transform.forward = dir;
            }
            

        }

        private void OnDisable()
        {
            UseScriptedEyes = false;
            ScriptedEyesTarget = null;
        }


    }

}
