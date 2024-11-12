using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped.UI
{
    public class PageManager : MonoBehaviour
    {
        public UnityAction OnOpened;
        public UnityAction OnClosed;

        List<GameObject> pages = new List<GameObject>();

        public bool IsOpen { get {  return pages.Count > 0; } }

        public GameObject CurrentPage 
        { 
            get 
            {
                if (!IsOpen)
                    return null;
                return pages[pages.Count-1]; 
            } 
        }

        // Start is called before the first frame update
        void Start()
        {
            HideAllPages();
        }
               

        void HideAllPages()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                CanvasGroup cg = transform.GetChild(i).GetComponent<CanvasGroup>();
                cg.alpha = 0;
                cg.blocksRaycasts = false;
                cg.gameObject.SetActive(false);
            }
        }

        public void Open(GameObject page)
        {
            GameObject current = null;
            if(pages.Count > 0)
                current = pages[pages.Count - 1];

            page.SetActive(true);
            CanvasGroup newC = page.GetComponent<CanvasGroup>();
            DOTween.To(() => newC.alpha, x => newC.alpha = x, 1f, .2f).onComplete += () => { newC.blocksRaycasts = true; };
            if (current)
            {
                var currC = current.GetComponent<CanvasGroup>();
                DOTween.To(() => currC.alpha, x => currC.alpha = x, 0f, .2f).onComplete += () => { currC.blocksRaycasts = false; currC.gameObject.SetActive(false); };
            }
            
            pages.Add(page);

            // First page, just opened
            if (pages.Count == 1)
                OnOpened?.Invoke();
          
        }

        public void Back()
        {
            if (pages.Count == 0)
                return;

            GameObject current = pages[pages.Count-1];
            var currC = current.GetComponent<CanvasGroup>();
            DOTween.To(() => currC.alpha, x => currC.alpha = x, 0f, .2f).onComplete += () => { currC.blocksRaycasts = false; currC.gameObject.SetActive(false); };
            pages.Remove(current);

            if (pages.Count > 0)
            {
                CanvasGroup newC = pages[pages.Count-1].GetComponent<CanvasGroup>();
                newC.gameObject.SetActive(true);
                DOTween.To(() => newC.alpha, x => newC.alpha = x, 1f, .2f).onComplete += () => { newC.blocksRaycasts = true; };
            }
            else
            {
                // Closed
                OnClosed?.Invoke();
            }
            
        }
    }

}
