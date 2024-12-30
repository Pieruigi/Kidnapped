using Kidnapped.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kidnapped
{
    public class PictureFall : MonoBehaviour, ISavable
    {
        [SerializeField]
        PlayerWalkInTrigger trigger;

        [SerializeField]
        int initialState = 0;// 0:not ready, 1:ready, 2:completed

        [SerializeField]
        GameObject picture;

        [SerializeField]
        Transform target;

        [SerializeField]
        AudioSource audioSource;

        [SerializeField]
        float delay;

        int state;
        Rigidbody rb;

        private void Awake()
        {
            rb = picture.GetComponent<Rigidbody>();

            string data = SaveManager.GetCachedValue(code);
            if (string.IsNullOrEmpty(data))
                data = initialState.ToString();
            Init(data);
        }

        private void OnEnable()
        {
            trigger.OnEnter += Fall;
        }

        private void OnDisable()
        {
            trigger.OnEnter -= Fall;
        }

        void Fall(PlayerWalkInTrigger trigger)
        {
            if (state != 1)
                return;

            trigger.gameObject.SetActive(false);
            state = 2;

            rb.isKinematic = false;
        }

        public void SetReadyState()
        {
            state = 1;
            trigger.gameObject.SetActive(true);
        }

        [SerializeField]
        string code;
        public string GetCode()
        {
            return code;
        }

        public string GetData()
        {
            return state.ToString();

        }

        public void Init(string data)
        {
            Debug.Log($"Init - {gameObject.name}:{data}");

            state = int.Parse(data);

            rb.isKinematic = true;

            if(state == 1)
                trigger.gameObject.SetActive(true);
            else
                trigger.gameObject.SetActive(false);

            if(state == 2)
            {
                picture.transform.position = target.position;
                picture.transform.rotation = target.rotation;
            }
        }

    }

}
