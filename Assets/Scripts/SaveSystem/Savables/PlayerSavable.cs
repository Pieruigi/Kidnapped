using EvolveGames;
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
            PlayerData d = (PlayerData)data;
            PlayerController.Instance.HasFlashlight = d.HasFlashlight;
            PlayerController.Instance.CanRunning = d.CanRun;
            PlayerController.Instance.CanCrouch = d.CanCrouch;
        }

        public override object GetData()
        {
            return new PlayerData() { Code = Code, HasFlashlight = PlayerController.Instance.HasFlashlight, CanCrouch = !PlayerController.Instance.CanCrouch, CanRun = !PlayerController.Instance.CanRunning };
        }

    }

}
