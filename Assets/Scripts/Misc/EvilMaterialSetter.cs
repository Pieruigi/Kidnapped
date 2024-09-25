using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class EvilMaterialSetter : MonoBehaviour
    {
        [SerializeField]
        List<Renderer> renderers;

        [SerializeField]
        Material normalMaterial;

        [SerializeField]
        Material evilMaterial;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetNormal()
        {
            foreach(Renderer renderer in renderers)
            {
                renderer.material = normalMaterial;
            }
        }

        public void SetEvil() 
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.material = evilMaterial;
            }
        }
    }

}
