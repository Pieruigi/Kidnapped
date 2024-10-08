using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class AnimationEventDispatcher : MonoBehaviour
    {
        public UnityAction<int> OnAnimationEvent;

         
        public void SendEvent(int id)
        {
            OnAnimationEvent?.Invoke(id);
        }
    }

}
