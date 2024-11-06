using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.UI
{
    public class AudioSettings : BaseMenu
    {

        [SerializeField]
        bool changes = false;

        public override void Back()
        {
            if (changes)
                PopUpManager.Instance.ShowPopUp("unapplied_changes", () => { base.Back(); }, () => { });
            else
                base.Back();
        }

        public void Apply()
        {

        }
    }

}
