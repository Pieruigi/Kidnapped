using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Kidnapped
{
    public class FlyingHeadOnBuildingSite : TripleStateProcessor
    {
        [SerializeField]
        PlayerWalkInAndLookTrigger trigger;

        [SerializeField]
        GameObject headPrefab;

        [SerializeField]
        Transform headTarget;

        [SerializeField]
        Transform headCompletedTarget;

        [SerializeField]
        GameObject sign;

        [SerializeField]
        Transform signTarget;

        [SerializeField]
        GameObject block;

        
        GameObject head;

        protected override void Awake()
        {
            base.Awake();

            Initialize(GetState());

        }

        private void OnEnable()
        {
            trigger.OnEnter += HandleOnTriggerEnter;
        }

        private void OnDisable()
        {
            trigger.OnEnter -= HandleOnTriggerEnter;
        }

        private async void HandleOnTriggerEnter(PlayerWalkInAndLookTrigger trigger)
        {
            trigger.gameObject.SetActive(false);

            await Task.Delay(700);

            // Spawn head
            head = Instantiate(headPrefab, headTarget.position, headTarget.rotation);

            head.transform.DORotateQuaternion(headCompletedTarget.rotation, .5f).SetEase(Ease.OutElastic);

            // Play stinger
            GameSceneAudioManager.Instance.PlayStinger(1);

            // Set the completed state
            base.SetState(State.Completed);

            Initialize(GetState());
            
        }

        void Initialize(State state)
        {
            switch (state)
            {
                case State.NotReady:
                    trigger.gameObject.SetActive(false);
                    block.SetActive(false);
                    break;
                case State.Ready:
                    trigger.gameObject.SetActive(true);
                    // Move sign
                    sign.transform.position = signTarget.position;
                    sign.transform.rotation = signTarget.rotation;
                    block.SetActive(true);
                    break;
                case State.Completed:
                    trigger.gameObject.SetActive(false);
                    // Move sign
                    sign.transform.position = signTarget.position;
                    sign.transform.rotation = signTarget.rotation;
                    // Show the stuck head
                    if(!head)
                        head = Instantiate(headPrefab, headCompletedTarget.position, headCompletedTarget.rotation);
                    block.SetActive(false);
                    break;
            }
        }

        public override void SetState(State state)
        {
            base.SetState(state);

            Initialize(GetState());
        }
    }

}
