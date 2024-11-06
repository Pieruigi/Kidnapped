using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Kidnapped.UI
{
    public class BaseMenu : MonoBehaviour
    {
        [SerializeField]
        bool startOpen = false;

        //[SerializeField]
        BaseMenu backMenu;

        //[SerializeField]
        CanvasGroup canvasGroup;



        protected virtual void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (startOpen)
            {
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
            }
                
        }

        public virtual void Open(BaseMenu backMenu = null)
        {
            if(backMenu != null)
                this.backMenu = backMenu;

            DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1f, .2f).onComplete += () => { canvasGroup.blocksRaycasts = true; } ;
        }


        public virtual void Close()
        {
            DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0f, .2f).onComplete += () => { canvasGroup.blocksRaycasts = false; };
            
            
        }

        public virtual void Back()
        {
            Close();
            if (backMenu)
            {
                backMenu.Open();
                backMenu = null;
            }
        }
    }

}
