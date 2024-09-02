using EvolveGames;
using Kidnapped.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;


namespace Kidnapped
{
    public class CutSceneController : MonoBehaviour, ISavable
    {
        public static UnityAction<CutSceneController> OnCompleted;

        [SerializeField]
        bool playOnEnter = false;
        public bool PlayOnEnter
        {
            get { return playOnEnter; }
        }
        

        //bool played = false;
        //public bool Played
        //{
        //    get { return played; }
        //}

        PlayableDirector director;
       

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
            
        }

        private void Start()
        {
            if(playOnEnter)// && !played)
                Play();
        }

        private void Update()
        {
            
        }

        private void OnEnable()
        {
            director.stopped += HandleOnDirectorStopped;
        }

        private void OnDisable()
        {
            director.stopped -= HandleOnDirectorStopped;
        }

        private void HandleOnDirectorStopped(PlayableDirector director)
        {
            //played = true;
            OnCompleted?.Invoke(this);
        }

        void Play()
        {

            director.Play();
        }

        public void Init(bool playOnEnter)
        {
            this.playOnEnter = playOnEnter;
        }

        #region save system
        [Header("SaveSystem")]
        [SerializeField]
        string code;

        public string GetCode()
        {
            return code;
        }

        public string GetData()
        {
            return playOnEnter.ToString();
        }

        public void Init(string data)
        {
            playOnEnter = bool.Parse(data);
        }

        #endregion
    }

}
