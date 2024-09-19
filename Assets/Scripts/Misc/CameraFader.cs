using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kidnapped
{
    public class CameraFader : Singleton<CameraFader>
    {
        public static float FadeOutTimeDefault = 1;
        public static float FadeInTimeDefault = 1;

        [SerializeField]
        Image panel;
        

        public void FadeIn()
        {
            FadeIn(FadeInTimeDefault);
        }

        public void FadeIn(float time)
        {
            panel.DOColor(new Color(0, 0, 0, 0), time);
        }

        public void FadeOut()
        {
            FadeOut(FadeOutTimeDefault);
        }

        public void FadeOut(float time)
        {
            panel.DOColor(new Color(0, 0, 0, 1), time);
        }

    }

}
