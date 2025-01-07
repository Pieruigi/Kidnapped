using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class ScaryFurnitureBlock : MultiStateSaver
    {
        [SerializeField]
        GameObject furniturePrefab;

        [SerializeField]
        Transform notReadyTarget;

        [SerializeField]
        Transform readyTarget;

        const int notActive = 0;
        const int notReady = 1;
        const int ready = 2;
        const int complete = 3;

        GameObject furniture;

        protected override void Awake()
        {
            base.Awake();

            Initialize(GetState());
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetReady()
        {
            SetState(ready);
            Initialize(GetState());
        }

        void Initialize(int state)
        {
            switch (state)
            {
                case notActive:
                    if(furniture)
                        Destroy(furniture);
                    break;
                case notReady:
                    if (!furniture)
                        furniture = Instantiate(furniturePrefab);
                    furniture.transform.position = notReadyTarget.position;
                    furniture.transform.rotation = notReadyTarget.rotation;
                    break;
                case ready:
                    if (!furniture)
                        furniture = Instantiate(furniturePrefab);
                    furniture.transform.position = readyTarget.position;
                    furniture.transform.rotation = readyTarget.rotation;
                    
                    break;
                case complete:
                    if (!furniture)
                        furniture = Instantiate(furniturePrefab);
                    furniture.transform.position = readyTarget.position;
                    furniture.transform.rotation = readyTarget.rotation;
                    break;
            }
        }
    }

}
