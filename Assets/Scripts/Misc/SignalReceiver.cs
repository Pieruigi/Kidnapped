using EvolveGames;
using Kidnapped.OldSaveSystem;
using Kidnapped.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class SignalReceiver : MonoBehaviour
    {
        public void SetPlayerInputOn()
        {
            Debug.Log("SetPlayerInputOn");
            PlayerController.Instance.PlayerInputEnabled = true;
        }

        public void SetPlayerInputOff()
        {
            PlayerController.Instance.PlayerInputEnabled = false;
        }

        public void SetWideScreenOn()
        {
            Camera.main.GetComponent<WideScreenController>().SetWideScreenOn();
        }

        public void SetWideScreenOff()
        {
            Camera.main.GetComponent<WideScreenController>().SetWideScreenOff();
        }

        public void SetGameData(GameDataSetter setter)
        {
            setter.SetData();
        }

        public void FlashlightOn()
        {
            Flashlight.Instance.SwitchOn();
        }

        public void FlashlightOff()
        {
            Flashlight.Instance.SwitchOff();
        }

        public void SubtitleOff()
        {
            SubtitleUI.Instance.Hide();
        }

        public void SetGameMenuAvailable()
        {
            InGameUIManager.Instance.SetMenuAvailable(true);
        }

        public void SetGameMenuUnavailable()
        {
            InGameUIManager.Instance.SetMenuAvailable(false);
        }
    }

}
