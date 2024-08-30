using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.SaveSystem
{
    public class PlayerSavable : Savable
    {
        protected override void Awake()
        {
            base.Awake();

            Code = "Player";
        }

        public override void SetData(object data)
        {
            
        }

        public override object GetData()
        {
            return new PlayerData() { Code = Code, HasFlashlight = true };
        }

    }

}
