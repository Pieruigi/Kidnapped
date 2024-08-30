using EvolveGames;
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
            Debug.Log("SetPlayerInputOff");
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
    }

}
