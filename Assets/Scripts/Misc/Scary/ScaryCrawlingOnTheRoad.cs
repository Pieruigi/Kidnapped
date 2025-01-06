using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Kidnapped
{
    public class ScaryCrawlingOnTheRoad : SimpleActivator
    {

        [SerializeField]
        PlayerWalkInAndLookTrigger trigger;

        [SerializeField]
        GameObject lilithPrefab;

        [SerializeField]
        Transform lilithTarget;

        [SerializeField]
        Transform[] lilithTargetGroup;

        GameObject lilith;

        List<GameObject> lilithGroup = new List<GameObject>();

        protected override void Awake()
        {
            base.Awake();

            Initialize();
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

            // Spawn Lilith
            lilith = Instantiate(lilithPrefab, lilithTarget.position, lilithTarget.rotation);

            // Set active
            lilith.SetActive(true);

            // Set animation 
            var anim = lilith.GetComponentInChildren<Animator>();
            anim.SetTrigger("Crawl");
            anim.speed = 1.5f;

            GameSceneAudioManager.Instance.PlayStinger(0);

            await Task.Delay(2500);

            FlashlightFlickerController.Instance.FlickerOnce(() =>
            {
                // Unspawn Lilith
                Destroy(lilith);

                gameObject.SetActive(false);

                Initialize();
            });
        }

        void Initialize()
        {
            
            Debug.Log("Do Init:" + gameObject.activeSelf);
            if(gameObject.activeSelf)
            {
                foreach(var t in lilithTargetGroup)
                {
                    var l = Instantiate(lilithPrefab, t.position, t.rotation);
                    lilithGroup.Add(l);
                    l.SetActive(true);
                    // Set animation
                    var anim = l.GetComponentInChildren<Animator>();
                    anim.SetTrigger("Agony");
                    // Randomize 
                    AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0);
                    anim.Play(currentState.fullPathHash, 0, UnityEngine.Random.Range(0f,1f));
                }
            }
            else
            {
                foreach(var l in lilithGroup)
                    Destroy(l);
            }
        }
    }

}
