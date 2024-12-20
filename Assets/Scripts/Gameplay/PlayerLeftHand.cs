using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class PlayerLeftHand : Singleton<PlayerLeftHand>
    {
        [SerializeField]
        Animator animator;

        string clueParamName = "Clue";
        string touchParamName = "Touch";

       
        public void PlayClueAnimation()
        {
            if(!animator.GetBool(clueParamName))
                animator.SetBool(clueParamName, true);
        }

        public void PlayIdleAnimation()
        {
            if (!animator)
                return;
            if (animator.GetBool(clueParamName))
                animator.SetBool(clueParamName, false);
        }

        public void PlayTouchAnimation()
        {
            animator.SetTrigger(touchParamName);
            animator.SetBool(clueParamName, false);
        }
    }

}
