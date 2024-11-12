using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped.UI
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField]
        AudioSource mainAudioSource;

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
        }
        
        private void OnDisable()
        {
            GameManager.OnSceneLoadingCompleted -= HandleOnSceneLoadingCompleted;
        }

        void HandleOnSceneLoadingProgress(float progress)
        {

        }

        void HandleOnSceneLoadingCompleted()
        {
            DOTween.To(() => mainAudioSource.volume, x => mainAudioSource.volume = x, 0, 2f);
        }
    }

}
