using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class WideScreenController : MonoBehaviour
    {
        [SerializeField]
        CameraFilterPack_TV_WideScreenHorizontal effect;

        [SerializeField]
        float wideValue = .55f;

        [SerializeField]
        float wideTime = 1;

        public void SetWideScreenOn()
        {
            DOTween.To(s => effect.Size = s, 1f, wideValue, wideTime);
        }

        public void SetWideScreenOff()
        {
            DOTween.To(s => effect.Size = s, wideValue, 1, wideTime);
        }
    }

}
