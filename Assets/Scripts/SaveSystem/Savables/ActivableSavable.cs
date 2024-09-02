using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.SaveSystem
{
    public class ActivableSavable : Savable
    {
        [SerializeField]
        GameObject controller;

        public override object GetData()
        {
            return new ActivableData() { Code = Code, Active = controller.activeSelf };
        }

        public override void SetData(object data)
        {
            ActivableData d = (ActivableData)data;
            if (d.Active && !controller.activeSelf)
                controller.SetActive(true);
            else if(!d.Active && controller.activeSelf)
                    controller.SetActive(false);
                
        }
    }

}

