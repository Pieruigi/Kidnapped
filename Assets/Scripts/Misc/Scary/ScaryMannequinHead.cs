using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Kidnapped
{
    public class ScaryMannequinHead : MonoBehaviour
    {
        //[SerializeField]
        //PlayerWalkInTrigger deactivator;

        float time = .5f;
        float elapsed = 0;
        bool inside = false;
        bool looking = false;
        bool startingLooking = false;
        float distance = 2f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!inside && !looking && !startingLooking)
            {
                elapsed += Time.deltaTime;
                if (elapsed > time)
                {
                    elapsed = 0;
                    Check();
                }
            }

            if (looking)
            {
                var target = Camera.main.transform.position - transform.position;
                transform.forward = Vector3.MoveTowards(transform.forward, target, Time.deltaTime * 5f);
            }
        }

        private void OnEnable()
        {
            //deactivator.OnEnter += (duration) => { GetComponentInParent<SimpleActivator>().gameObject.SetActive(false); };
        }

        void Check()
        {
            var playerPosition = PlayerController.Instance.transform.position;
            playerPosition.y = 0;
            var thisPosition = transform.position;
            thisPosition.y = 0;
            if (Vector3.Distance(playerPosition, thisPosition) < distance)
            {
                inside = true;
                
                StartCoroutine(StartLooking());
            }
        }

        IEnumerator StartLooking()
        {
            startingLooking = true;
            yield return new WaitForSeconds(1f);
            startingLooking = false;
            looking = true;
            GameSceneAudioManager.Instance.PlayStinger(2);
        }
    }

}
