using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kidnapped.UI
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField]
        AudioSource mainAudioSource;

        [SerializeField]
        Image bar;

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
            GameManager.OnSceneLoadingCompleted += HandleOnSceneLoadingCompleted;
            GameManager.OnSceneLoadingProgress += HandleOnSceneLoadingProgress;
            bar.fillAmount = 0;
        }
        
        private void OnDisable()
        {
            GameManager.OnSceneLoadingCompleted -= HandleOnSceneLoadingCompleted;
            GameManager.OnSceneLoadingProgress -= HandleOnSceneLoadingProgress;
        }

        void HandleOnSceneLoadingProgress(float progress)
        {
            bar.fillAmount = progress;
        }

        void HandleOnSceneLoadingCompleted()
        {
            bar.fillAmount = 1f;
            DOTween.To(() => mainAudioSource.volume, x => mainAudioSource.volume = x, 0, 1f);
        }
    }

}
