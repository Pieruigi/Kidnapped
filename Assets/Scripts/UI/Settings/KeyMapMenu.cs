using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.UI
{
    public class KeyMapMenu : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void NotYet()
        {
            PopUpManager.Instance.ShowInfoPopUp("no_key_map");
        }
    }

}
