using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class RandomRotator : MonoBehaviour
    {
        [SerializeField]
        Vector3 speedMin;
       
        [SerializeField]
        Vector3 speedMax;

        Vector3 axis;
        

        private void Awake()
        {
            Randomize();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        { 
            
            transform.rotation *= Quaternion.Euler(axis * Time.deltaTime);
        }

        void Randomize()
        {
            axis = new Vector3(Random.Range(speedMin.x, speedMax.x), Random.Range(speedMin.y, speedMax.y), Random.Range(speedMin.z, speedMax.z));
            axis.x *= Random.Range(0, 2) == 0 ? 1 : -1;
            axis.y *= Random.Range(0, 2) == 0 ? 1 : -1;
            axis.z *= Random.Range(0, 2) == 0 ? 1 : -1;
        }

        public void SetSpeedMinAndMax(Vector3 speedMin, Vector3 speedMax)
        {
            this.speedMin = speedMin;
            this.speedMax = speedMax;
            Randomize();
        }
    }

}
