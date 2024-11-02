using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Kidnapped
{
    public class DialogController : MonoBehaviour
    {
        [System.Serializable]
        class Sentence
        {
            [SerializeField]
            public Speaker speaker;

            [SerializeField]
            public int index;

            [SerializeField]
            public float delay;
        }

        [SerializeField]
        List<Sentence> sentences;

        int currentIndex = -1;

        UnityAction callback;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        private void HandleOnSentenceComplete(Speaker arg0)
        {
            if (currentIndex >= sentences.Count)
            {
                callback?.Invoke();
                return; // No more talk
            }

            Debug.Log($"TEST - {arg0} completed");

            currentIndex++;
            Talk();
        }

        async void Talk()
        {
            var sentence = sentences[currentIndex];

            // Wait for delay
            await Task.Delay(TimeSpan.FromSeconds(sentence.delay));

            // Talk
            VoiceManager.Instance.Talk(sentence.speaker, sentence.index, HandleOnSentenceComplete);
        }



        public void Play(UnityAction OnCompletedCallback = null)
        {
            // Set callback
            callback = OnCompletedCallback;

            // Get the first sentence
            currentIndex = 0;
            Talk();
        }

        
    }

}
