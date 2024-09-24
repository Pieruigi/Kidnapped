using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    

    public class BellController : MonoBehaviour
    {
        public static UnityAction<BellController> OnRing;

        [SerializeField]
        ObjectInteractor interactor;

        Animator animator;
        AudioSource audioSource;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

      
        private void OnEnable()
        {
            interactor.OnInteraction += HandleOnInteraction;
        }

        private void OnDisable()
        {
            interactor.OnInteraction -= HandleOnInteraction;
        }

        private async void HandleOnInteraction()
        {
    
            animator.SetTrigger("Play");
            await Task.Delay(500);
            audioSource.Play();

            OnRing?.Invoke(this);
        }

        public void Play()
        {
            HandleOnInteraction();
        }
    }

}
