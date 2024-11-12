using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.UI
{
    public class LoadingCanvas : MonoBehaviour
    {
        [SerializeField]
        GameObject panel;

        CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = panel.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            panel.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            GameManager.OnSceneLoadingStarted += Show;
        }

        private void OnDisable()
        {
            GameManager.OnSceneLoadingStarted -= Show;
        }

        public void Show()
        {
            panel.SetActive(true);
            // Show panel

            DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1f, .2f);
        }
    }

}
