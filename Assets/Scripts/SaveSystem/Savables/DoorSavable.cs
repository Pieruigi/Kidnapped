using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Kidnapped.SaveSystem
{
    public class DoorSavable : Savable
    {
        [SerializeField]
        DoorController controller;

        public override void SetData(object data)
        {
            DoorData dd = (DoorData)data;
            controller.Init(dd.IsLocked, dd.IsOpen, dd.InteractionDisabled);
        }

        public override object GetData()
        {
            return new DoorData() { Code = Code, IsLocked = controller.IsLocked, IsOpen = controller.IsOpen, InteractionDisabled = controller.InteractionDisabled };

        }
    }

}
